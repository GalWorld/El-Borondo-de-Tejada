using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public static class SceneController
{
    public static void LoadNewScene(string nameOfTheSceneToLoad)
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(nameOfTheSceneToLoad, LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public static void LoadPhotoScene(string nameOfTheSceneToLoad, Material skyboxMaterial)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(nameOfTheSceneToLoad, LoadSceneMode.Single);

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (scene.name == nameOfTheSceneToLoad)
            {
                ApplySkybox(skyboxMaterial);
                GameController.Instance.SetupPhotoSceneUI();
                SceneManager.sceneLoaded -= OnSceneLoaded; 
            }
        };
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DynamicGI.UpdateEnvironment();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private static void ApplySkybox(Material skyboxMaterial)
    {
        if (skyboxMaterial == null)
        {
            Debug.LogError("❌ No skybox material provided!");
            return;
        }

        RenderSettings.skybox = skyboxMaterial;
        DynamicGI.UpdateEnvironment();
    }
}
