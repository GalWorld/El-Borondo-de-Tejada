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

// Clase serializable para representar un logro individual
[System.Serializable]
public class AchievementCat
{
    public string name = "New Cat";
    public bool unlocked = false;
}

public class GameController : MonoBehaviour
{
    [SerializeField] private GameStateWrapper gameState; // This will be shown in the Inspector
    public static GameController Instance { get; private set; }
    private HashSet<string> collectedPhotoIDs = new HashSet<string>();
    
    // Lista serializada de logros para configurar desde el Inspector
    [SerializeField] private List<AchievementCat> achievementDefinitions = new List<AchievementCat>();
    
    // Lista para almacenar el estado de los logros en tiempo de ejecución
    private List<bool> achievements = new List<bool>();

    public GameState CurrentState 
    { 
        get => gameState.state; 
        private set
        {
            gameState.state = value;
        }
    }

    public static event System.Action<GameState> OnGameStateChanged;
    public static event System.Action<int> OnAchievementUnlocked;

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

    // Methods for verify if the list have elements
    private void EnsureAchievementListSize()
    {
        while (achievements.Count < achievementDefinitions.Count)
        {
            achievements.Add(false);
        }
        
        if (achievements.Count > achievementDefinitions.Count)
        {
            achievements.RemoveRange(achievementDefinitions.Count, 
                                    achievements.Count - achievementDefinitions.Count);
        }
    }

    // Method for unlock an achivement
    public void UnlockAchievement(int achievementID)
    {
        EnsureAchievementListSize();
        
        // check if the ID is valid
        if (achievementID < 0 || achievementID >= achievements.Count)
        {
            Debug.LogError($"❌ Invalid achievement ID: {achievementID}. Max ID is {achievements.Count - 1}");
            return;
        }
        
        // check if the achive was unlock
        if (!achievements[achievementID])
        {
            achievements[achievementID] = true;
            
            // update the achive boolean value in the List element
            achievementDefinitions[achievementID].unlocked = true;
            
            Debug.Log($"🏆 Achievement unlocked: {achievementDefinitions[achievementID].name}");
            SaveAchievements();
            
            // Notify other components
            OnAchievementUnlocked?.Invoke(achievementID);
        }
    }
    
    // Check if the achive is unlocked
    public bool IsAchievementUnlocked(int achievementID)
    {
        EnsureAchievementListSize();
        
        if (achievementID < 0 || achievementID >= achievements.Count)
        {
            Debug.LogError($"❌ Invalid achievement ID: {achievementID}. Max ID is {achievements.Count - 1}");
            return false;
        }
        
        return achievements[achievementID];
    }
    private void SaveGame()
    {
        SaveSystem.SaveCollectedPhotos(collectedPhotoIDs);
    }
    
    private void SaveAchievements()
    {
        SaveSystem.SaveAchievements(achievements);
    }

    private void LoadGame()
    {
        collectedPhotoIDs = SaveSystem.LoadCollectedPhotos();
        achievements = SaveSystem.LoadAchievements();
        
        EnsureAchievementListSize();
        
        // update the achive boolean value in the achive definition
        for (int i = 0; i < achievements.Count; i++)
        {
            achievementDefinitions[i].unlocked = achievements[i];
        }
    }

    private void DeleteSave()
    {
        SaveSystem.DeleteSave();
        collectedPhotoIDs.Clear();
        
        // Reiniciar los logros
        for (int i = 0; i < achievementDefinitions.Count; i++)
        {
            achievementDefinitions[i].unlocked = false;
        }
        
        achievements.Clear();
        EnsureAchievementListSize();
        
        Debug.Log("🗑️ Save file deleted and data reset.");
    }

    // 🔹 This method can be assigned to a UI button
    public void RequestDeleteSave()
    {
        DeleteSave();
    }
    
    // Método para obtener la lista completa de logros (para UI)
    public List<bool> GetAllAchievements()
    {
        EnsureAchievementListSize();
        return new List<bool>(achievements);
    }
    
    // Método para obtener el número total de logros
    public int GetTotalAchievements()
    {
        return achievementDefinitions.Count;
    }
    
    // Método para obtener todas las definiciones de logros (para UI)
    public List<AchievementCat> GetAchievementDefinitions()
    {
        return achievementDefinitions;
    }
}