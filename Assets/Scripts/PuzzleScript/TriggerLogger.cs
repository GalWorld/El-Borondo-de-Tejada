using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLogger : MonoBehaviour
{
    public bool isActivated = false; 
    public string targetTag = "Target"; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            isActivated = true; 
            //Debug.Log($"{gameObject.name} Activated for {other.name}");
            PuzzleManager.Instance.CheckPuzzleCompletion(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            isActivated = false; 
            PuzzleManager.Instance.CheckIsOff();
            //Debug.Log($"{gameObject.name} Activated for {other.name}");
        }
    }
}
