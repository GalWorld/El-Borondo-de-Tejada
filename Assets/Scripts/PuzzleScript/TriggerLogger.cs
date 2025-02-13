using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLogger : MonoBehaviour
{
    //public static TriggerLogger Instance; 
    public bool isActivated = false; 
    public string targetTag = "Target"; 
    private AudioSource audioSource;
    [SerializeField] private AudioClip LampCorrect;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = LampCorrect;
        audioSource.loop = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            isActivated = true; 
            audioSource.Play();
            //Debug.Log($"{gameObject.name} Activated for {other.name}");
            PuzzleManager.Instance.CheckPuzzleCompletion(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            isActivated = false; 
            StopThisAudio();
            PuzzleManager.Instance.CheckIsOff();
            //Debug.Log($"{gameObject.name} Activated for {other.name}");
        }
    }
    public void StopThisAudio()
    {
        audioSource.Stop();
    }
}
