using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{
    public static void LoadNewScene(string nameOfTheSceneToLoad)
    {
        SceneManager.LoadScene(nameOfTheSceneToLoad, LoadSceneMode.Single);
    }
}
