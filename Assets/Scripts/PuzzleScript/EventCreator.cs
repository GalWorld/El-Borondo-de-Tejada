using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCreator : MonoBehaviour, IInteractable
{
    [SerializeField] TargetMover targetMover; // TargetMover Reference
    [SerializeField] private string interactText;

    public void Interact(Transform interactorTransform)
    {
        if (targetMover != null)
        {
            targetMover.MoveToNextPoint();
        }
    }

    public string GetInteractText()
    {
        return interactText;
    }
}
