using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private float interactCooldown = 0.5f;
    [SerializeField] private GameObject chatInterfaceUI;
    [SerializeField] private GameObject playerInteractUI;

    private PlayerInputs playerInputs;
    private bool canInteract = true;
    private IInteractable currentInteractable;

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

    private void Update()
    {
        UpdateInteractableObject();
    }

    private void UpdateInteractableObject()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        IInteractable closestInteractable = null;

        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                closestInteractable = interactable;
                break;
            }
        }

        if (closestInteractable != currentInteractable)
        {
            if (currentInteractable != null)
            {
                OnInteractableExit(currentInteractable);
            }

            currentInteractable = closestInteractable;

            if (currentInteractable != null)
            {
                OnInteractableEnter(currentInteractable);
            }
        }
    }

    private void OnInteractableEnter(IInteractable interactable)
    {
        Debug.Log($"Entered range of: {interactable.GetInteractText()}");

        canInteract = true;
        if (playerInteractUI != null)
        {
            playerInteractUI.SetActive(true);
        }
    }

    private void OnInteractableExit(IInteractable interactable)
    {
        Debug.Log($"Exited range of: {interactable.GetInteractText()}");

        NPCInteractable npc = interactable as NPCInteractable;
        if (npc != null)
        {
            if (npc.IsTyping())
            {
                npc.StopTyping();
            }

            if (chatInterfaceUI != null)
            {
                chatInterfaceUI.SetActive(false);
            }

            if (playerInteractUI != null)
            {
                playerInteractUI.SetActive(false);
            }

            GameController.Instance.SetGameState(GameState.Playing);
        }
    }

    public IInteractable GetInteractableObject()
    {
        return currentInteractable;
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (canInteract && currentInteractable != null)
        {
            TryInteract();
        }
    }

    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        canInteract = true;
    }

    private void TryInteract()
    {
        canInteract = false;

        if (currentInteractable != null)
        {
            currentInteractable.Interact(transform);
        }

        StartCoroutine(ResetInteractCooldown());
    }

    private IEnumerator ResetInteractCooldown()
    {
        yield return new WaitForSeconds(interactCooldown);
        canInteract = true;
    }
}
