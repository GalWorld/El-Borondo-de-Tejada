using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatManager : MonoBehaviour
{
    [SerializeField] private GameObject catInCurrentScene; 
    [SerializeField] private string nextSceneName; 
    [SerializeField] private GameObject puzzleCanvas;

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

        StartCoroutine(ActivateCatInNextScene());
    }

    public void GoBackToPlay()
    {
        if (puzzleCanvas != null)
        {
            puzzleCanvas.SetActive(false); // Deactivate the canvas
        }

        Cursor.lockState = CursorLockMode.Locked; // Deactivate the mouse
    }

    private IEnumerator ActivateCatInNextScene()
    {
        yield return new WaitForSeconds(5); // Wait 5 seconds to start

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName); // Load the next scene by name

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene nextScene = SceneManager.GetSceneByName(nextSceneName); // Validate if the scene is valid
        if (nextScene.IsValid())
        {
            GameObject[] rootObjects = nextScene.GetRootGameObjects(); // Look for every object in the scene
            foreach (GameObject obj in rootObjects)
            {
                if (obj.CompareTag("Cat"))
                {
                    obj.SetActive(true); // Activate the cat
                    break;
                }
            }
        }

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
