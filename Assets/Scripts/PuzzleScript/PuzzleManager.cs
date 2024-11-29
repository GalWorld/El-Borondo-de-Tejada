using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance; 

    public TriggerLogger[] triggerLogger; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckPuzzleCompletion()
    {
        foreach (TriggerLogger cruz in triggerLogger)
        {
            if (!cruz.isActivated)
            {
                return;
            }
        }

        PuzzleFinished();
    }

    private void PuzzleFinished()
    {
        Debug.Log("Puzzle Completed");
    }
}
