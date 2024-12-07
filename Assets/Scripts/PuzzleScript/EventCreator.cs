using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCreator : MonoBehaviour, IInteractable
{
    [SerializeField] TargetMover targetMover; // TargetMover Reference
    [SerializeField] private EventCreator[] otherLamps; //Other lamps array
    [SerializeField] private string interactText;

    public void Interact(Transform interactorTransform)
    {
        if (targetMover != null)
        {
            targetMover.MoveToNextPoint();
        }

        //It looks for every lamp in the array 
        foreach (EventCreator lamp in otherLamps)
        {   
            //it confirms if the array is not null
            if (lamp != null && lamp.targetMover != null)
            {
                lamp.targetMover.MoveToNextPoint();
            }
        }
    }

    public string GetInteractText()
    {
        return interactText;
    }
}
