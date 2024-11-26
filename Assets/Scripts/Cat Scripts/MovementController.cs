using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [Header("Scene References")]
    public Transform cameraTransform;

    [Header("Movement Values")]
    [Range(0.0f, 10.0f)]
    [SerializeField] private float walkSpeed= 4;
    [Range(0.0f, 10.0f)]
    [SerializeField] private float runSpeed = 6;

    [Range(0.0f, 10.0f)]
    [SerializeField] private float rotationSpeed = 1;

    public bool isRunning;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 moveDirection;
    private PlayerInputs playerInputs;

    private void Awake() {
        playerInputs = new();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate() {
        HandleMovement();
        RotateCharacter();
    }

    #region Player Input Management
    private void OnEnable() {
        playerInputs.Player.Enable();

        playerInputs.Player.Movement.performed += OnMove;
        playerInputs.Player.Movement.canceled += CancelMove;

        playerInputs.Player.Run.performed += OnRun;
        playerInputs.Player.Run.canceled += OnRun;
    }

    private void OnDisable() {
        playerInputs.Player.Disable();

        playerInputs.Player.Movement.performed -= OnMove;
        playerInputs.Player.Movement.canceled -= CancelMove;

        playerInputs.Player.Run.performed -= OnRun;
        playerInputs.Player.Run.canceled -= OnRun;
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
    #endregion

    private void HandleMovement()
    {
        //if isRunning speed gonna be runSpeed
        float currentSpeed = isRunning ? runSpeed:walkSpeed;

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
            //we use .Slerp because the movement is smoother than other functions that are linear
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

}
