using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    // Handles player inputs from the input system.
    private PlayerInputs playerInputs;
    
    private void Awake() {
        playerInputs = new();
    }

    private void OnEnable() {
        playerInputs.Player.Enable();

        playerInputs.Player.Movement.performed += OnPause;
    }

    private void OnDisable() {
        playerInputs.Player.Disable();

        playerInputs.Player.Movement.performed -= OnPause;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        
        if(context.performed)
        {
            Pause();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Play()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
