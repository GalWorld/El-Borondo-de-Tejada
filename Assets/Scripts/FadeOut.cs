using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    [SerializeField] private float timeToFade;
    public Image panel;

    private void Awake() {
        StartCoroutine(FadeOutBlack());
    }

    public IEnumerator FadeOutBlack()
    {
        // Get the current color of the Image
        Color startColor = panel.color;
        
        float timeElapsed = 0f;
        while (timeElapsed < timeToFade)
        {
            float t = timeElapsed / timeToFade;
            
            // Lerp alpha from its start value to 0
            float newAlpha = Mathf.Lerp(startColor.a, 0f, t);
            panel.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure alpha is exactly 0 at the end of the fade
        panel.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        panel.gameObject.SetActive(false);
    }
}
