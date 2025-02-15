using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoBudgetCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject budgetUI;

    public void ActivateViewOfBudget()
    {
        budgetUI.SetActive(true);
    }

        public void ExitBudgetPhotos()
    {
        GameController.Instance.SetGameState(GameState.Playing);
        budgetUI.SetActive(false);
    }
}
