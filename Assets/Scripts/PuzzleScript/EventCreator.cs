using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCreator : MonoBehaviour, IInteractable
{
    [SerializeField] TargetMover targetMover; // TargetMover Reference

    public void Interact(){
        //Debug.Log("Touch it");

        if (targetMover != null)
        {
            targetMover.MoveToNextPoint();
        }
    }
}
