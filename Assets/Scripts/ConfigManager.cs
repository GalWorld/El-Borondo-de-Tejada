using System.IO;
using UnityEngine;

public static class ConfigManager
{
    private static string configPath = Path.Combine(Application.streamingAssetsPath, "config.json");
    private static string encryptionKey;

    public static string GetEncryptionKey()
    {
        if (!string.IsNullOrEmpty(encryptionKey))
        {
            return encryptionKey; 
        }

        if (File.Exists(configPath))
        {
            string json = File.ReadAllText(configPath);
            ConfigData configData = JsonUtility.FromJson<ConfigData>(json);
            encryptionKey = configData.encryptionKey;
            return encryptionKey;
        }
        else
        {
            Debug.LogError("⚠️ Config file not found! Encryption key missing.");
            return null;
        }
    }

    [System.Serializable]
    private class ConfigData
    {
        public string encryptionKey;
    }
}
