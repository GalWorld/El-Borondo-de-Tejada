using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// Data structure for storing collected photo IDs
[System.Serializable]
public class SaveData
{
    public List<string> collectedPhotos = new List<string>();
}

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/photoProgress.dat";
    private static readonly string encryptionKey = ConfigManager.GetEncryptionKey(); 
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

        string json = JsonUtility.ToJson(new SaveData { collectedPhotos = encryptedIDs });
        File.WriteAllText(savePath, json);
        Debug.Log($"✅ Progress saved at {savePath}");
    }

    public static HashSet<string> LoadCollectedPhotos()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("⚠️ No save file found, returning empty collection.");
            return new HashSet<string>();
        }

        if (new FileInfo(savePath).Length == 0)
        {
            Debug.Log("⚠️ Save file exists but is empty, returning empty collection.");
            return new HashSet<string>();
        }

        if (string.IsNullOrEmpty(encryptionKey))
        {
            Debug.LogError("❌ Encryption key is missing. Cannot load data.");
            return new HashSet<string>();
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        
        HashSet<string> decryptedIDs = new HashSet<string>();
        foreach (string encryptedID in data.collectedPhotos)
        {
            decryptedIDs.Add(DecryptID(encryptedID));
        }

        return decryptedIDs;
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
