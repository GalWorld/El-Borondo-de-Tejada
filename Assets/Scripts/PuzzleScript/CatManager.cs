using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatManager : MonoBehaviour
{
    [SerializeField] GameObject catInCurrentScene; 
    [SerializeField] private string nextSceneName; 

    public void SwapCats()
    {
        // Desactivamos el Cat de la escena actual
        if (catInCurrentScene != null)
        {
            catInCurrentScene.SetActive(false);
        }

        // Cargar la siguiente escena y activar el Cat en ella
        StartCoroutine(ActivateCatInNextScene());
    }

    private IEnumerator ActivateCatInNextScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene nextScene = SceneManager.GetSceneByName(nextSceneName);
        if (nextScene.IsValid())
        {
            GameObject[] rootObjects = nextScene.GetRootGameObjects();
            foreach (GameObject obj in rootObjects)
            {
                if (obj.CompareTag("Cat")) 
                {
                    obj.SetActive(true); 
                    break;
                }
            }
        }

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
