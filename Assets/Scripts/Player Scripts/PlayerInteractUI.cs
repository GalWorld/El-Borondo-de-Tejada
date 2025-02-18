using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractUI : MonoBehaviour
{
    private GameObject uiImage;
    private Text interactText;
    [SerializeField] private PlayerInteract playerInteract;

    private void Awake() 
    {
        uiImage = transform.GetChild(0).gameObject;
        interactText = transform.GetChild(1).GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        if (GameController.Instance.CurrentState != GameState.Playing)
        {
            Hide();
            return;
        }
        
        if (playerInteract.GetInteractableObject() != null)
        {
            Show(playerInteract.GetInteractableObject());
        } else 
        {
            Hide();
        }
    }

    private void Show(IInteractable interactable)
    {
        uiImage.SetActive(true);
        interactText.gameObject.SetActive(true);
        interactText.text = interactable.GetInteractText();
    }

    private void Hide()
    {
        uiImage.SetActive(false);
        interactText.gameObject.SetActive(false);
    }
}
