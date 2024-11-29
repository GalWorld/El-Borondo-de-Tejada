using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCreator : MonoBehaviour, IInteractable
{
    public TargetMover targetMover; // TargetMover Reference

    public void Interact(){
        //Debug.Log("Tocalo");

        if (targetMover != null)
        {
            targetMover.MoveToNextPoint();
        }
    }
}
