using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactRange;

    private PlayerInputs playerInputs;
    private bool isInteracting;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        playerInputs.Player.Enable();

        playerInputs.Player.EnvInteraction.performed += OnInteractPerformed;
        playerInputs.Player.EnvInteraction.canceled += OnInteractCanceled;
    }

    private void OnDisable()
    {
        playerInputs.Player.Disable();

        playerInputs.Player.EnvInteraction.performed -= OnInteractPerformed;
        playerInputs.Player.EnvInteraction.canceled -= OnInteractCanceled;
    }

    private void FixedUpdate() 
    {
        if (isInteracting)
        {
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact(transform);
                }
            }
        }
    }

    public IInteractable GetInteractableObject()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                return interactable;
            }
        }
        return null;
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        isInteracting = true;
    }

    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        isInteracting = false;
    }
}
