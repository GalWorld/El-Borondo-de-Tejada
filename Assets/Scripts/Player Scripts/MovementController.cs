using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [Header("Scene References")]
    [Tooltip("The transform of the camera used for determining movement direction.")]
    public Transform cameraTransform;
    private CinemachineBrain cinemachineBrain;

    [Header("Movement Values")]
    [Tooltip("The speed at which the player walks.")]
    [Range(0.0f, 10.0f)]
    [SerializeField] private float walkSpeed = 4;

    [Tooltip("The speed at which the player runs.")]
    [Range(0.0f, 10.0f)]
    [SerializeField] private float runSpeed = 6;

    [Tooltip("The speed at which the player rotates.")]
    [Range(0.0f, 10.0f)]
    [SerializeField] private float rotationSpeed = 1;

    [Tooltip("Indicates whether the player is running.")]
    public bool isRunning;

    [Header("Jump Values")]
    [Tooltip("The force applied to the player when jumping.")]
    [SerializeField] private float jumpForce;

    [Tooltip("The waiting time to be able to execute the method jump again.")]
    [SerializeField] private float cooldownJump;

    [Tooltip("The distance used for raycasting to check ground collision.")]
    [SerializeField] private float raycastDistance;

    [Header("Slope Handling")]
    [SerializeField] private float slopeRaycastDistance = 1.0f; 
    [SerializeField] private float slopeRotationSpeed = 20.0f;  


    [Header("Sounds Controller")]
    [SerializeField] private SoundsCatController soundsCatController;
    [SerializeField] private GameObject audioSource;
    [SerializeField] private float walkPitch;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    // Tracks if the player is currently able to jump.
    private bool canJump = false;
    private bool isCooldownJumpActive = false;

    // The Rigidbody component used for physics-based movement.
    private Rigidbody rb;

    // Stores the input from the player for movement.
    private Vector2 moveInput;

    // The calculated direction the player will move in.
    private Vector3 moveDirection;

    // Handles player inputs from the input system.
    private PlayerInputs playerInputs;

    // References the animator
    private Animator animator;

    //Handle the current player speed
    private float currentSpeed;

    //Speed fot the animator
    private float animSpeed = 0f;

    //Auxiliar speed for SmoothDamp
    private float speedVelocity = 0f;

    private void Awake()
    {
        playerInputs = new();
        OnApplicationFocus(cursorLocked); //This metod hide the mouse cursor during the game
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        soundsCatController = GetComponent<SoundsCatController>();
        cameraTransform = Camera.main.transform;
        cinemachineBrain = cameraTransform.GetComponent<CinemachineBrain>();
    }

    private void FixedUpdate()
    {
        if (GameController.Instance.CurrentState == GameState.Interacting)
        {
            LockCamera();
            CancelAllMovement();
            SetCursorState(false);
        }
        else
        {
            UnlockCamera();
            SetCursorState(true);
            HandleMovement();
            RotateCharacter();
            ValidationJump();
            UpdateAnimator();
        }
    }

    #region Player Input Management
    private void OnEnable()
    {
        playerInputs.Player.Enable();

        playerInputs.Player.Movement.performed += OnMove;
        playerInputs.Player.Movement.canceled += CancelMove;

        playerInputs.Player.Run.performed += OnRun;
        playerInputs.Player.Run.canceled += OnRun;

        playerInputs.Player.Jump.started += OnJump;
    }

    private void OnDisable()
    {
        playerInputs.Player.Disable();

        playerInputs.Player.Movement.performed -= OnMove;
        playerInputs.Player.Movement.canceled -= CancelMove;

        playerInputs.Player.Run.performed -= OnRun;
        playerInputs.Player.Run.canceled -= OnRun;

        playerInputs.Player.Jump.started -= OnJump;
    }
    #endregion

    #region Events
    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        soundsCatController.SetAudio(0, walkPitch, true);
        audioSource.SetActive(true);
    }
    private void CancelMove(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
        audioSource.SetActive(false);
    }
    private void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.performed;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && canJump && !isCooldownJumpActive)
        {
            Jump();
        }
    }
    #endregion

    private void UpdateAnimator()
    {
        // Calculate the target speed based on the movement magnitude
        float rawSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;

        // Normalize the target speed
        float targetSpeed = Mathf.Clamp01(rawSpeed / runSpeed);

        // Smoothly transition the animation speed using SmoothDamp
        animSpeed = Mathf.SmoothDamp(animSpeed, targetSpeed, ref speedVelocity, 0.2f);

        // Correct small values to avoid fluctuations
        if (Mathf.Abs(animSpeed) < 0.01f) animSpeed = 0f;

        // Update the parameter Speed in the Animator
        animator.SetFloat("SpeedAnim", animSpeed);
        // Update the parameter isRunning in teh Animator
        animator.SetBool("IsRunning", isRunning);
    }

    private void HandleMovement()
    {
        //if isRunning speed gonna be runSpeed
        currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Get the foward and right direction of the camera
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        //Ignore the Y component for keep the movement in XZ plane
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        // Normalize for uniform movement speed
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the movement direction based on the camera's orientation and input
        moveDirection = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

        // Update the Rigidbody's velocity with the calculated movement direction
        rb.velocity = moveDirection * currentSpeed + new Vector3(0, rb.velocity.y, 0);
    }

    private void RotateCharacter()
    {
        // Get the slope rotation
        Quaternion slopeRotation = GetSlopeRotation();
        
        if (moveDirection != Vector3.zero)
        {
            // Calculate the rotation based on movement direction
            Quaternion moveRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            // Combine both rotations
            transform.rotation = Quaternion.Slerp(transform.rotation, moveRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation, slopeRotationSpeed * Time.deltaTime);
        }
        else
        {
            // Apply slope rotation when idle
            transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation, slopeRotationSpeed * Time.deltaTime);
        }
    }

    private void ValidationJump()
    {
        // get a global -y component transforming the local direction to global with TransformDirection
        Vector3 dwn = transform.TransformDirection(Vector3.down);

        // create the raycast origin with a little desphase
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;

        Debug.DrawRay(rayOrigin, dwn * raycastDistance, Color.yellow);

        // Check if the player is also not in the air (velocity in y axis is near 0)
        bool isInAir = Mathf.Abs(rb.velocity.y) > 0.1f;

        // Check if the raycast hits a collider in the down direction, ensuring we're on the ground
        bool isGrounded = Physics.Raycast(rayOrigin, dwn, raycastDistance, gameObject.layer);

        // Can jump only if both conditions are true: we're on the ground and not in the air
        canJump = isGrounded && !isInAir;
    }

    private void Jump()
    {
        //applies an upward force to the player
        rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
        //Updating the trigger of Jump
        animator.SetTrigger("Jump");

        // Start cooldown
        StartCoroutine(JumpCooldownCoroutine());
    }

    private IEnumerator JumpCooldownCoroutine()
    {
        isCooldownJumpActive = true;
        yield return new WaitForSeconds(cooldownJump);
        isCooldownJumpActive = false;
    }

    private Quaternion GetSlopeRotation()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, slopeRaycastDistance))
        {
            Vector3 surfaceNormal = hit.normal;

            if (hit.collider.CompareTag("Terrain"))
            {
                // Align the "up" vector with the surface normal
                return Quaternion.FromToRotation(transform.up, surfaceNormal) * transform.rotation;
            }
        }
        // Default rotation if no slope is detected
        return transform.rotation;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void CancelAllMovement()
    {
        moveInput = Vector2.zero;
        moveDirection = Vector3.zero;
        rb.velocity = Vector3.zero;
        
        isRunning = false;
        animator.SetFloat("SpeedAnim", 0f);
        animator.SetBool("IsRunning", false);
    }

    private void LockCamera()
    {
        if (cinemachineBrain != null)
        {
            cinemachineBrain.enabled = false; // Disable Cinemachine control
        }
    }

    private void UnlockCamera()
    {
        if (cinemachineBrain != null)
        {
            cinemachineBrain.enabled = true; // Re-enable Cinemachine control
        }
    }
}
