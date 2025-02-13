using UnityEngine;

public class ChatInterfaceUI : MonoBehaviour
{
    private void Awake() 
    {
        Hide();  
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
