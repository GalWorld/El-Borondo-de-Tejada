using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/photoProgress.dat";
    private static readonly string encryptionKey = "MySecretKey12345"; // Debe ser exactamente de 16, 24 o 32 caracteres.

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
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = new byte[16]; // Vector de inicialización vacío para simplificar.

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

[System.Serializable]
public class SaveData
{
    public List<string> collectedPhotos = new List<string>();
}
