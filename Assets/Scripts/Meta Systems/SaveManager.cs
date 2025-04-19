using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Stores all data that persists in a save file.
/// </summary>
[Serializable]
public class SaveManager : MonoBehaviour
{
    private static string folderPath;
    private static string savePath;

    public static SaveData data = new();

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);

        folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        // TODO: support multiple save files?
        savePath = Path.Combine(folderPath, "save.json");
    }

    /// <summary>
    /// Saves data to the disk.
    /// </summary>
    public void Save()
    {
        string saveData = JsonUtility.ToJson(data);
        
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        try
        {
            File.WriteAllText(savePath, saveData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Exception while saving game: {e}");
        }
    }

    /// <summary>
    /// Loads data from the disk.
    /// </summary>
    public void Load()
    {
        string saveData;
        try
        {
            saveData = File.ReadAllText(savePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Exception while loading save data: {e}");
            return;
        }

        data = JsonUtility.FromJson<SaveData>(saveData);
    }
}
