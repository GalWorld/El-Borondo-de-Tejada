using UnityEngine;

public class PhotoBook : MonoBehaviour, IInteractable
{
    private PhotoBudgetCanvasController PhotoBudgetCanvasController;
    private GameObject  PhotoBudgetUI;

    void Start()
    {
        PhotoBudgetCanvasController = FindAnyObjectByType<PhotoBudgetCanvasController>();
        PhotoBudgetUI = PhotoBudgetCanvasController.transform.GetChild(0).gameObject;
    }

    public void Interact(Transform interactorTransform)
    {
        if (PhotoBudgetCanvasController != null)
        {
            PhotoBudgetUI.SetActive(true);
            GameController.Instance.SetGameState(GameState.Interacting);
        }
    }

    public string GetInteractText()
    {
        return "Presiona 'E' para ver las fotos de tu Borondo";
    }
}
