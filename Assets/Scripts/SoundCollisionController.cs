using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollisionController : MonoBehaviour
{
    private AudioSource audioSource;
    GameObject child;
    void Start()
    {
        child = transform.GetChild(0).gameObject;
        audioSource = child.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(PlaySound());
        }
    }

    IEnumerator PlaySound()
    {
        child = transform.GetChild(0).gameObject;
        child.SetActive(true);
        yield return new WaitForSeconds(audioSource.clip.length);
        child.SetActive(false);
    }

    
}
