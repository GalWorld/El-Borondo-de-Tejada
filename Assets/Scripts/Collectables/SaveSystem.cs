using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// Data structure for storing collected photo IDs and achievements
[System.Serializable]
public class SaveData
{
    public List<string> collectedPhotos = new List<string>();
    public List<bool> achievements = new List<bool>();
}

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/photoProgress.dat";
    private static readonly string encryptionKey = ConfigManager.GetEncryptionKey(); 
    
    // Method for save ONLY photos
    public static void SaveCollectedPhotos(HashSet<string> collectedPhotoIDs)
    {
        if (string.IsNullOrEmpty(encryptionKey))
        {
            Debug.LogError("❌ Encryption key is missing. Cannot save data.");
            return;
        }

        List<string> encryptedIDs = new List<string>();
        foreach (string id in collectedPhotoIDs)
        {
            encryptedIDs.Add(EncryptID(id));
        }

        SaveData saveData = new SaveData { collectedPhotos = encryptedIDs };

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, json);
        Debug.Log($"✅ Progress saved at {savePath}");
    }

    // Method for save ONLY achivements
    public static void SaveAchievements(List<bool> achievements)
    {
        if (string.IsNullOrEmpty(encryptionKey))
        {
            Debug.LogError("❌ Encryption key is missing. Cannot save data.");
            return;
        }

        SaveData existingData = LoadSaveDataDirectly();
        if (existingData == null)
        {
            existingData = new SaveData();
        }

        existingData.achievements = achievements;
        
        string json = JsonUtility.ToJson(existingData);
        File.WriteAllText(savePath, json);
        Debug.Log($"✅ Achievements saved at {savePath}");
    }

    // Method for directly load the save data
    private static SaveData LoadSaveDataDirectly()
    {
        if (!File.Exists(savePath))
        {
            return null;
        }

        if (new FileInfo(savePath).Length == 0)
        {
            return null;
        }

        if (string.IsNullOrEmpty(encryptionKey))
        {
            Debug.LogError("❌ Encryption key is missing. Cannot load data.");
            return null;
        }

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public static HashSet<string> LoadCollectedPhotos()
    {
        SaveData data = LoadSaveDataDirectly();
        if (data == null)
        {
            Debug.Log("⚠️ No save file found or is empty, returning empty collection.");
            return new HashSet<string>();
        }
        
        HashSet<string> decryptedIDs = new HashSet<string>();
        foreach (string encryptedID in data.collectedPhotos)
        {
            decryptedIDs.Add(DecryptID(encryptedID));
        }

        return decryptedIDs;
    }

    // Method for load the achivements
    public static List<bool> LoadAchievements()
    {
        SaveData data = LoadSaveDataDirectly();
        if (data == null || data.achievements == null)
        {
            Debug.Log("⚠️ No achievements found, returning empty list.");
            return new List<bool>();
        }
        
        return data.achievements;
    }

    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("🗑️ Save file deleted successfully.");
        }
        else
        {
            Debug.Log("⚠️ No save file found to delete.");
        }
    }

    private static string EncryptID(string id)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = new byte[16]; 

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] inputBytes = Encoding.UTF8.GetBytes(id);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

            return Convert.ToBase64String(encryptedBytes);
        }
    }

    private static string DecryptID(string encryptedID)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = new byte[16]; 

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] encryptedBytes = Convert.FromBase64String(encryptedID);
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}