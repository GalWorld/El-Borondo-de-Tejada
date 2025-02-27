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

[System.Serializable]
public class GameStateWrapper
{
    public GameState state;
}

public class GameController : MonoBehaviour
{
    [SerializeField] private GameStateWrapper gameState; // This will be shown in the Inspector
    public static GameController Instance { get; private set; }
    private HashSet<string> collectedPhotoIDs = new HashSet<string>();

    public GameState CurrentState 
    { 
        get => gameState.state; 
        private set
        {
            gameState.state = value;
        }
    }

    public static event System.Action<GameState> OnGameStateChanged;

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
    }

    public void SetGameState(GameState newState)
    {
        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState); 
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
        GameObject photoCanvas = FindAnyObjectByType<Canvas>()?.gameObject;
        if (photoCanvas == null)
        {
            Debug.LogError("❌ No PhotoScene UI found!");
            return;
        }

        Button returnButton = photoCanvas.transform.Find("BackLobby")?.GetComponent<Button>();
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
        CameraPhotoSceneController cameraController = Camera.main?.GetComponent<CameraPhotoSceneController>();
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
}
