using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundSettingsPrefs : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private List<AudioClip> audioClips;


    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
        audioSource  = GetComponent<AudioSource>();
    }

     private void OnEnable()
    {
        // subscribe to the event
        SceneManager.activeSceneChanged += ChangeSceneMusic;
    }

    private void OnDisable()
    {
        // Unsubscribe to the event
        SceneManager.activeSceneChanged -= ChangeSceneMusic;
    }

    private void ChangeSceneMusic(Scene previousScene, Scene newScene)
    {
        switch (newScene.name)
        {
            case "Menu":
                audioSource.clip = audioClips[0];
                audioSource.Play();
                break;
                
            case "Lobby2":
                audioSource.clip = audioClips[1];
                audioSource.Play();
                break;

            case "Cerro3Cruces":
                audioSource.clip = audioClips[2];
                audioSource.Play();
                break;

            default:
                audioSource.clip = audioClips[0];
                audioSource.Play();
                break;
        }
    }
}
