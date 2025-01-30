using System.Collections.Generic;
using UnityEngine;

public class SaveData : ScriptableObject
{
    public List<string> partyMembers;

    /// <summary>
    /// Pulls data from the scene to create a single serializable save object.
    /// </summary>
    public void InitializeSaveData()
    {
        partyMembers = new List<string>();
        foreach (GhostIdentity ghost in FindObjectOfType<PartyManager>().GetGhostMajorList())
        {
            partyMembers.Add(ghost.name);
        }
    }

    public void ApplySaveData()
    {
        // TODO
    }
}