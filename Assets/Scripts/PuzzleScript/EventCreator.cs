using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCreator : MonoBehaviour, IInteractable
{
    [SerializeField] TargetMover targetMover; // TargetMover Reference

    public void Interact(Transform interactorTransform)
    {
        if (targetMover != null)
        {
            targetMover.MoveToNextPoint();
        }
    }

    public string GetInteractText()
    {
        throw new System.NotImplementedException();
    }
}
