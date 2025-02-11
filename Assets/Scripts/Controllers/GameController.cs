using UnityEngine;
using System.Collections.Generic;

public enum GameState
{
    Menu,
    Playing,
    Pause
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

    public void CollectPhoto(PhotoData photo)
    {
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
}
