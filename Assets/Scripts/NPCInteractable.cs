using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject chatInterfaceUI;
    [SerializeField] private GameObject playerInteractUI;
    [SerializeField] private TextMeshProUGUI  ownerNameText;
    [SerializeField] private TextMeshProUGUI  chatText; 
    [SerializeField] private Image ownerImage;
    [SerializeField] private string interactText;
    [SerializeField] private DialogueMessage[] messages; 

    private int currentMessageIndex = 0;
    private Coroutine typingCoroutine;

    public void Interact(Transform interactorTransform)
    {
        if (chatInterfaceUI != null)
        {
            if (!chatInterfaceUI.activeSelf)
            {
                chatInterfaceUI.SetActive(true);
                playerInteractUI.SetActive(false);
                ShowCurrentMessage();
            }
            else
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                    typingCoroutine = null;
                    CompleteCurrentMessage();
                }
                else
                {
                    AdvanceMessage();
                }
            }
        }
    }

    public string GetInteractText()
    {
        return interactText;
    }

    private void ShowCurrentMessage()
    {
        if (messages.Length > 0)
        {
            DialogueMessage currentMessage = messages[currentMessageIndex];

            if (ownerNameText != null)
                ownerNameText.text = currentMessage.ownerName;

            if (ownerImage != null)
                ownerImage.sprite = currentMessage.ownerImage;

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine); 

            typingCoroutine = StartCoroutine(TypeText(currentMessage.messageContent));
        }
    }

    private IEnumerator TypeText(string messageContent)
    {
        chatText.text = ""; 
        foreach (char c in messageContent)
        {
            chatText.text += c; 
            yield return new WaitForSeconds(0.05f); 
        }
        typingCoroutine = null; 
    }

    private void CompleteCurrentMessage()
    {
        DialogueMessage currentMessage = messages[currentMessageIndex];
        chatText.text = currentMessage.messageContent;
    }

    private void AdvanceMessage()
    {
        currentMessageIndex++;

        if (currentMessageIndex < messages.Length)
        {
            ShowCurrentMessage();
        }
        else
        {
            chatInterfaceUI.SetActive(false);
            playerInteractUI.SetActive(true);
            currentMessageIndex = 0; 
        }
    }
}
