using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatManager : MonoBehaviour
{
    [SerializeField] private GameObject catInCurrentScene; 
    [SerializeField] private string nextSceneName; 
    [SerializeField] private GameObject puzzleCanvas;
    [SerializeField] private int catAchiveID;

    public static CatManager Instance { get; private set; } // Singleton Instance

    private void Awake()
    {
        // Check if there's already an instance of this object
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
            return;
        }
    }

    public void SwapCats()
    {
        if (catInCurrentScene != null)
        {
            catInCurrentScene.SetActive(false); // Deactivate the current cat
        }

        if (puzzleCanvas != null)
        {
            puzzleCanvas.SetActive(true); // Activate Canvas
        }

        Cursor.lockState = CursorLockMode.None; // Activate mouse

        GameController.Instance.UnlockAchievement(catAchiveID);
    }

    public void GoBackToPlay()
    {
        SceneController.LoadNewScene("Lobby");

        Cursor.lockState = CursorLockMode.Locked; // Deactivate the mouse
    }
}
