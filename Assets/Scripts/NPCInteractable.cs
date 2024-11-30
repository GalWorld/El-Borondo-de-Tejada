using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject chatBubblePrefab;
    [SerializeField] private string interactText;

    private GameObject currentChatBubble;

    public void Interact(Transform interactorTransform)
    {
        if (currentChatBubble == null)
        {
            currentChatBubble = Instantiate(chatBubblePrefab, transform.position, Quaternion.identity);
        }
        else if (!currentChatBubble.activeSelf)
        {
            currentChatBubble.SetActive(true);
        }
        else
        {
            currentChatBubble.SetActive(false);
        }
    }

    public string GetInteractText()
    {
        return interactText;
    }
}
