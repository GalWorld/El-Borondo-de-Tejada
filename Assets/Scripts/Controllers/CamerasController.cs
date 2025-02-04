using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasController : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private GameObject initialWorldCamera;
    [SerializeField] private GameObject puzzleCamera;
    [SerializeField] private GameObject freeLookCamera;


    [Header("Animation Values")]
    [SerializeField] private float animationTime;

    private void Start() {
        StartCoroutine(TurnCamera(initialWorldCamera, animationTime, false));
        StartCoroutine(TurnCamera(freeLookCamera, animationTime-1, true));
    }


    //method for turn on/off a object before X time
    IEnumerator TurnCamera(GameObject obj, float time, bool value)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(value);
    }

    private void OnTriggerStay(Collider other) {
        if(other.CompareTag("Player"))
        {
            puzzleCamera.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            puzzleCamera.SetActive(false);
        }
    }




}
