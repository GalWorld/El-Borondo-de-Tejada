using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance; 

    [SerializeField] CatManager catManager;

    [SerializeField] TriggerLogger[] triggerLogger; 
    private AudioSource audioSource;
    [SerializeField] private AudioClip LampOn;
    [SerializeField] private AudioClip LampOff;
    [SerializeField] private AudioClip Win;
    [SerializeField] private AudioClip Chains;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
        foreach (TriggerLogger cross in triggerLogger)
        {
            audioSource.PlayOneShot(LampOn);
            if (!cross.isActivated)
            {
                return;
            }
        }

        PuzzleFinished();
    }
    public void CheckIsOff()
    {
        audioSource.PlayOneShot(LampOff);
    }
    public void ChainsMove()
    {
        audioSource.PlayOneShot(Chains);
    }
    private void PuzzleFinished()
    {
        //Debug.Log("Puzzle Completed");
        audioSource.PlayOneShot(Win);
          if (catManager != null)
        {
            catManager.SwapCats();
        }
    }
}
