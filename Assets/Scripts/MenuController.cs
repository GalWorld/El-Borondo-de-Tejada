using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Tooltip("The name of the scene to load")]
    [SerializeField] private string nameloadScene;

    [SerializeField] private float timeToFade;
    public Image panel;

    public void StartGame()
    {
        Debug.Log("start a fadeout");
        StartCoroutine(FadeIn());
        
    }

    public IEnumerator FadeIn()
    {
        panel.gameObject.SetActive(true);
        // Get the current color of the Image
        Color startColor = panel.color;
        
        float timeElapsed = 0f;
        while (timeElapsed < timeToFade)
        {
            float t = timeElapsed / timeToFade;
            
            // Lerp alpha from its start value to 0
            float newAlpha = Mathf.Lerp(startColor.a, 1f, t);
            panel.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure alpha is exactly 0 at the end of the fade
        panel.color = new Color(startColor.r, startColor.g, startColor.b, 1f);
        SceneController.LoadNewScene(nameloadScene);
    }
}
