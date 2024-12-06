using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    

    public void ChangeVolume()
    {
        AudioListener.volume = slider.value;
    }
}
