using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasController : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private GameObject initialWorldCamera;
    [SerializeField] private GameObject puzzleCamera;

    [Header("Animation Values")]
    [SerializeField] private float animationTime;

    private void Start() {
        StartCoroutine(TurnCamera(initialWorldCamera, animationTime, false));
    }


    //method for turn on/off a object before X time
    IEnumerator TurnCamera(GameObject obj, float time, bool value)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(value);
    }




}
