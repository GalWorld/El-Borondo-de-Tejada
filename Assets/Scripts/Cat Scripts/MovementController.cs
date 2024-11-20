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
    [SerializeField] private float speed;

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
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    #region Player Input Management
    private void OnEnable() {
        playerInputs.Player.Enable();

        playerInputs.Player.Movement.performed += OnMove;
        playerInputs.Player.Movement.canceled += CancelMove;
    }

    private void OnDisable() {
        playerInputs.Player.Disable();

        playerInputs.Player.Movement.performed -= OnMove;
        playerInputs.Player.Movement.canceled -= CancelMove;
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
    #endregion

    private void HandleMovement()
    {
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
        rb.velocity = moveDirection * speed + new Vector3(0, rb.velocity.y, 0);
    }

}
