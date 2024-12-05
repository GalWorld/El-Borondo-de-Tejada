using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsCatController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> catAudioClips;

    /*This method change the current audio and the speed reproduction (pitch) 
    pitch parameter is optional, have default value as 1
    isLoop parameter is optional, have default value as false*/
    public void SetAudio(int audioIndex, float audioPitch = 1, bool isLoop = false)
    {
        audioSource.clip = catAudioClips[audioIndex];
        audioSource.pitch = audioPitch;
        audioSource.loop = isLoop;
    }
}
