using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [Header("Scene References")]
    [Tooltip("The transform of the camera used for determining movement direction.")]
    public Transform cameraTransform;

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

    [Tooltip("The distance used for raycasting to check ground collision.")]
    [SerializeField] private float raycastDistance;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    // Tracks if the player is currently able to jump.
    private bool canJump = false;

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


    private void Awake() {
        playerInputs = new();
        OnApplicationFocus(cursorLocked); //This metod hide the mouse cursor during the game
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); 
        cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate() {
        HandleMovement();
        RotateCharacter();
        ValidationJump();
        UpdateAnimator();
    }

    #region Player Input Management
    private void OnEnable() {
        playerInputs.Player.Enable();

        playerInputs.Player.Movement.performed += OnMove;
        playerInputs.Player.Movement.canceled += CancelMove;

        playerInputs.Player.Run.performed += OnRun;
        playerInputs.Player.Run.canceled += OnRun;

        playerInputs.Player.Jump.performed += OnJump;
    }

    private void OnDisable() {
        playerInputs.Player.Disable();

        playerInputs.Player.Movement.performed -= OnMove;
        playerInputs.Player.Movement.canceled -= CancelMove;

        playerInputs.Player.Run.performed -= OnRun;
        playerInputs.Player.Run.canceled -= OnRun;

        playerInputs.Player.Jump.performed -= OnJump;
    }
    #endregion

    #region Events
    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    private void CancelMove(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }
    private void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.performed;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed && canJump)
        {
            Jump();
        }
    }
    #endregion

    private void UpdateAnimator()
    { 
        // Directly assign movement input to the animator parameters
        float horizontal = moveInput.x; // Horizontal input (-1 to 1)
        float vertical = moveInput.y;   // Vertical input (-1 to 1)

        // Update the parameters in the Animator
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
    }

    private void HandleMovement()
    {
        //if isRunning speed gonna be runSpeed
        currentSpeed = isRunning ? runSpeed:walkSpeed;

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
        if (moveDirection != Vector3.zero)
        {
            // calculates the rotation with respect to the direction of motion
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            // smooth interpolate between the actual rotation to target rotation 
            // use .Slerp because the movement is smoother than other linear functions 
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void ValidationJump()
    {
        // get a global -y component transforming the local direction to global with TransformDirection
        Vector3 dwn = transform.TransformDirection(Vector3.down);

        // create the raycast origin with a little desphase
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;

        Debug.DrawRay(rayOrigin, dwn * raycastDistance, Color.yellow); 

        /* if the raycast hit with other collider in direction down can Jump 
        additionally the raycast gonna ignore the player layer*/
        canJump = Physics.Raycast(rayOrigin, dwn, raycastDistance, gameObject.layer);
    }
    private void Jump()
    {
        //applies an upward force to the player
        rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

}
