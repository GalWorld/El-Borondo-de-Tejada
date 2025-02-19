using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum GameState
{
    Menu,
    Playing,
    Pause,
    Interacting
}

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    private HashSet<string> collectedPhotoIDs = new HashSet<string>();

    public GameState CurrentState { get; private set; } = GameState.Menu;

    public enum detectedDevice
    {
        desktop,
        mobile,
        console
    }
    public detectedDevice deviceType;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        LoadGame();
        // DeleteSave();
        CurrentState = GameState.Menu;

        Debug.Log(SystemInfo.deviceType);
        SwitchDevice();
    }

    public void SetGameState(GameState newState)
    {
        CurrentState = newState;
    }

    public bool TryPauseGame()
    {
        if (CurrentState == GameState.Playing)
        {
            return true;
        }
        
        return false;
    }

    public void CollectPhoto(PhotoData photo)
    {
        if (CurrentState != GameState.Interacting)
        {
            Debug.Log("⚠️ Cannot collect photo because game is not in Collecting state!");
            return;
        }

        if (!collectedPhotoIDs.Contains(photo.id))
        {
            collectedPhotoIDs.Add(photo.id);
            Debug.Log($"📸 Collected new photo: {photo.title}");
            SaveGame();
        }
    }

    public void SetupPhotoSceneUI()
    {
        GameObject photoCanvas = FindAnyObjectByType<Canvas>().gameObject;
        Debug.Log(photoCanvas.name);
        if (photoCanvas == null)
        {
            Debug.LogError("❌ No PhotoScene UI found!");
            return;
        }

        Button returnButton = photoCanvas.transform.Find("BackLobby")?.GetComponent<Button>();
        Debug.Log(returnButton.name);
        if (returnButton != null)
        {
            returnButton.onClick.RemoveAllListeners();
            returnButton.onClick.AddListener(ReturnToLobby);
        }

        EnableMouseControl();
    }

    public void ReturnToLobby()
    {
        SceneController.LoadNewScene("Lobby");
        SetGameState(GameState.Playing);
    }

    private void EnableMouseControl()
    {
        CameraPhotoSceneController cameraController = Camera.main.GetComponent<CameraPhotoSceneController>();
        if (cameraController != null)
        {
            cameraController.allowMouseLook = true;
        }
    }

    public HashSet<string> GetCollectedPhotoIDs()
    {
        return collectedPhotoIDs;
    }

    public bool HasCollectedPhoto(string photoID)
    {
        return collectedPhotoIDs.Contains(photoID);
    }

    private void SaveGame()
    {
        SaveSystem.SaveCollectedPhotos(collectedPhotoIDs);
    }

    private void LoadGame()
    {
        collectedPhotoIDs = SaveSystem.LoadCollectedPhotos();
    }

    private void DeleteSave()
    {
        SaveSystem.DeleteSave();
        collectedPhotoIDs.Clear();
        Debug.Log("🗑️ Save file deleted and data reset.");
    }

    // 🔹 This method can be assigned to a UI button
    public void RequestDeleteSave()
    {
        DeleteSave();
    }

     private void SwitchDevice()
    {
        switch (SystemInfo.deviceType)
        {
            case DeviceType.Handheld:
                deviceType = detectedDevice.mobile;
                break;
            
            case DeviceType.Desktop:
                deviceType = detectedDevice.desktop;
                break;

            case DeviceType.Console:
                deviceType = detectedDevice.console;
                break;
            
            case DeviceType.Unknown:
                deviceType = detectedDevice.console;
                break;
            
            default:
                deviceType = detectedDevice.desktop;
                break;
        }
    }
}
