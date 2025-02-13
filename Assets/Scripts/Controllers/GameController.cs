using UnityEngine;
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
        CurrentState = GameState.Menu;
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
            Debug.LogWarning("⚠️ Cannot collect photo because game is not in Collecting state!");
            return;
        }

        if (!collectedPhotoIDs.Contains(photo.id))
        {
            collectedPhotoIDs.Add(photo.id);
            SaveGame();
            Debug.Log($"📸 Collected new photo: {photo.title}");
        }
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
