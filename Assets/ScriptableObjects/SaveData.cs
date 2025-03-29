using System.Collections.Generic;
using UnityEngine;

public class SaveData : ScriptableObject
{
    private readonly PartyManager partyManager = FindObjectOfType<PartyManager>();

    public List<string> partyMembers;

    /// <summary>
    /// Pulls data from the scene to create a single serializable save object.
    /// </summary>
    public void InitializeSaveData()
    {
        partyMembers = new List<string>();
        foreach (GhostIdentity ghost in partyManager.GetGhostMajorList())
        {
            partyMembers.Add(ghost.name);
        }
    }

    /// <summary>
    /// Modifies the game state to match the save data.
    /// </summary>
    public void ApplySaveData()
    {
        partyManager.GetGhostMajorList().Clear();
        GhostIdentity[] ghosts = FindObjectsOfType<GhostIdentity>();
        foreach (string member in partyMembers)
        {
            foreach (GhostIdentity ghost in ghosts)
            {
                if (ghost.name == member)
                {
                    partyManager.TryAddGhostToParty(ghost);
                    break;
                }
            }
        }
    }
}