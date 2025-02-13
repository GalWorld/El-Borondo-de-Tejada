using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class SaveSystem
{
    // Define the file path where the save data will be stored
    private static string savePath = Application.persistentDataPath + "/photoProgress.dat";
    private static readonly string encryptionKey = "MySecretKey12345"; 

    public static void SaveCollectedPhotos(HashSet<string> collectedPhotoIDs)
    {
        List<string> encryptedIDs = new List<string>();
        foreach (string id in collectedPhotoIDs)
        {
            encryptedIDs.Add(EncryptID(id));
        }

        string json = JsonUtility.ToJson(new SaveData { collectedPhotos = encryptedIDs });
        File.WriteAllText(savePath, json);
        Debug.Log($"Progress saved at {savePath}");
    }

    public static HashSet<string> LoadCollectedPhotos()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            
            HashSet<string> decryptedIDs = new HashSet<string>();
            foreach (string encryptedID in data.collectedPhotos)
            {
                decryptedIDs.Add(DecryptID(encryptedID));
            }

            return decryptedIDs;
        }
        else
        {
            Debug.LogWarning("No save file found, returning empty collection.");
            return new HashSet<string>();
        }
    }

    public static void DeleteSave()
    {
        // Check if the save file exists and delete it
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save file deleted.");
        }
    }

    private static string EncryptID(string id)
    {
        using (Aes aes = Aes.Create())
        {
            // Set the encryption key and initialization vector (IV)
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = new byte[16]; // Using a zeroed IV (not the best security practice)

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            // Convert the input ID into bytes and encrypt it
            byte[] inputBytes = Encoding.UTF8.GetBytes(id);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

            // Convert the encrypted bytes to Base64 string for storage
            return Convert.ToBase64String(encryptedBytes);
        }
    }

    private static string DecryptID(string encryptedID)
    {
        // Set the encryption key and initialization vector (IV)
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = new byte[16]; // Must match the IV used during encryption

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            // Convert the Base64 string back to bytes and decrypt it
            byte[] encryptedBytes = Convert.FromBase64String(encryptedID);
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            
            // Convert the decrypted bytes back to a string (original ID)
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}

// Data structure for storing collected photo IDs
[System.Serializable]
public class SaveData
{
    public List<string> collectedPhotos = new List<string>();
}
