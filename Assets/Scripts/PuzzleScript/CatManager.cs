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
        // Cargar la próxima escena de manera aditiva
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);

        // Esperar a que la escena se cargue completamente
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Buscar y activar el Cat en la nueva escena
        Scene nextScene = SceneManager.GetSceneByName(nextSceneName);
        if (nextScene.IsValid())
        {
            GameObject[] rootObjects = nextScene.GetRootGameObjects();
            foreach (GameObject obj in rootObjects)
            {
                if (obj.CompareTag("Cat")) // Usa el Tag para identificar al Cat
                {
                    obj.SetActive(true); // Activa el Cat en la nueva escena
                    break;
                }
            }
        }

        // Opcional: Cerrar la escena actual
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
