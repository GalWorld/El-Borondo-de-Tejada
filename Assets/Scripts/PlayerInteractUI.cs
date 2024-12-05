using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractUI : MonoBehaviour
{
    private GameObject uiImage;
    private Text interactText;
    private PlayerInteract playerInteract;
    public bool _IsInteract;

    private void Awake() 
    {
        uiImage = transform.GetChild(0).gameObject;
        interactText = transform.GetChild(1).GetComponent<Text>();
        playerInteract = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteract>();
    }

    private void FixedUpdate()
    {
        if (playerInteract.GetInteractableObject() != null)
        {
            Show(playerInteract.GetInteractableObject());
        } else 
        {
            Hide();
        }

        if (_IsInteract) 
        { 
            Hide(); 
            _IsInteract = false;
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
