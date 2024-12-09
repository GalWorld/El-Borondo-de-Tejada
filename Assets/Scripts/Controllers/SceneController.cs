using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{
    public static void LoadNewScene(string nameOfTheSceneToLoad)
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(nameOfTheSceneToLoad, LoadSceneMode.Single);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DynamicGI.UpdateEnvironment();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
