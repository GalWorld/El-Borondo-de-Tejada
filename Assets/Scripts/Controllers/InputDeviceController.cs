using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDeviceController : MonoBehaviour
{
    [SerializeField] private GameObject mobileHUD;
    private GameController gameController;
    private void Start() 
    {
        gameController = FindObjectOfType<GameController>();

        if(gameController.deviceType == GameController.detectedDevice.mobile)
        {
            mobileHUD.SetActive(true);
        }
    }


}
