using System;

/// <summary>
/// Save Data module for saving how much each ghost has talked to each other in hub
/// </summary>
[Serializable]
public class GhostToGhostProgressSaveData
{
    public string relationName; // eg north_eva
    public int progress; // how far along they are in their relationship

    public GhostToGhostProgressSaveData(string relationName)
    {
        this.relationName = relationName;
        progress = 0;
    }
}
