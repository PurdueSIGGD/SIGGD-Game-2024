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

    public void Awake()
    {
        DontDestroyOnLoad(this);

        folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        // TODO: support multiple save files?
        savePath = Path.Combine(folderPath, "save.json");
    }

    /// <summary>
    /// Saves data to the disk.
    /// </summary>
    public void Save()
    {
        SaveData save = new();
        save.InitializeSaveData();
        string saveData = JsonUtility.ToJson(save);
        
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

        SaveData save = JsonUtility.FromJson<SaveData>(saveData);
        save.ApplySaveData();
    }
}
