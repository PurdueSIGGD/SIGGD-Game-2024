using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Stores all data that persists in a save file.
/// </summary>
[Serializable]
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance = null;

    private static string folderPath;
    private static string savePath;

    public static SaveData data = new();

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        // TODO: support multiple save files?
        savePath = Path.Combine(folderPath, "save.json");
    }

    private void SetGhostsSkillPts(ref int[] ptarr, string g)
    {
        ptarr[0] = data.ghostLevel[g];
        for (int i = 0; i < data.ghostSkillPts[g].Length; i++)
        {
            ptarr[i+1] = data.ghostSkillPts[g][i];
        }
    }

    /// <summary>
    /// Saves data to the disk.
    /// </summary>
    public void Save()
    {
        data.saveGhostNames = new List<string>();

        foreach (string g in data.ghostLevel.Keys)
        {
            /*List<int> ghostData = new List<int>();
            ghostData.Add(data.ghostLevel[g]);
            ghostData.AddRange(data.ghostSkillPts[g]);
            data.saveGhostData.Add(ghostData);*/
            if (g.Contains("North"))
            {
                SetGhostsSkillPts(ref data.northSkillPts, g);
            }
            else if (g.Contains("Eva"))
            {
                SetGhostsSkillPts(ref data.evaSkillPts, g);
            }
            else if (g.Contains("Yume"))
            {
                SetGhostsSkillPts(ref data.yumeSkillPts, g);
            }
            else if (g.Contains("Akihito"))
            {
                SetGhostsSkillPts(ref data.akihitoSkillPts, g);
            }
            else if (g.Contains("Silas"))
            {
                SetGhostsSkillPts(ref data.silasSkillPts, g);
            }
            else if (g.Contains("Aegis"))
            {
                SetGhostsSkillPts(ref data.aegisSkillPts, g);
            }
        }

        string saveData = JsonUtility.ToJson(data, true);

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

    private void LoadGhostSkills(ref int[] pointArr, string name)
    {
        data.ghostLevel.Add(name, pointArr[0]);
        int[] ghostSkillPts = new int[7];
        for (int i = 1; i < pointArr.Length; i++)
        {
            ghostSkillPts[i - 1] = pointArr[i];
        }
        data.ghostSkillPts.Add(name, ghostSkillPts);
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
            Debug.LogWarning($"Exception while loading save data: {e}");
            return;
        }

        data = JsonUtility.FromJson<SaveData>(saveData);

        LoadGhostSkills(ref data.northSkillPts, "North-Police_Chief");
        LoadGhostSkills(ref data.evaSkillPts, "Eva-Idol");
        LoadGhostSkills(ref data.yumeSkillPts, "Yume-Seamstress");
        LoadGhostSkills(ref data.akihitoSkillPts, "Akihito-Samurai");
        LoadGhostSkills(ref data.silasSkillPts, "Silas-PlagueDoc");
        LoadGhostSkills(ref data.aegisSkillPts, "Aegis-King");
    }
}
