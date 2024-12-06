using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatManager : MonoBehaviour
{
    [SerializeField] GameObject catInCurrentScene; 
    [SerializeField] private string nextSceneName; 
    [SerializeField] GameObject puzzleCanvas;
    public static CatManager instance; //Instance for scene change

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // Keep instance
        }
        else
        {
            Destroy(gameObject); // Destroy other instances 
        }
    }
    public void SwapCats()
    {
        catInCurrentScene.SetActive(false);//Deactivate the current cat
        
        puzzleCanvas.SetActive(true);//Activate Canvas

        Cursor.lockState = CursorLockMode.None; //Activate mouse
        

        StartCoroutine(ActivateCatInNextScene());
    }

    public void GoBackToPlay()
    {
        //this function is for the play button

        puzzleCanvas.SetActive(false); //Deactivate the canvas

        Cursor.lockState = CursorLockMode.Locked;//Deactivate the mouse

    }

    private IEnumerator ActivateCatInNextScene()
    {
        yield return new WaitForSeconds(5); //Wait 5 seconds to start

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);//Load the next scene depends on the name

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene nextScene = SceneManager.GetSceneByName(nextSceneName); //It have to validate if the scene is valid
        if (nextScene.IsValid())
        {
            GameObject[] rootObjects = nextScene.GetRootGameObjects(); //Look for ever object in the scene
            foreach (GameObject obj in rootObjects)
            {
                if (obj.CompareTag("Cat")) 
                {
                    obj.SetActive(true); //Activate the cat 
                    break;
                }
            }
        }

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
