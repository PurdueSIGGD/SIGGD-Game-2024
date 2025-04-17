/// Party management script that tracks every active major ghost
/// Attaches to Player

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PartyManager : MonoBehaviour
{
    public static PartyManager instance;

    [SerializeField] int ghostLimit; // maximum number of ghosts player can wield at one time

    private Dictionary<string, GhostIdentity> ghostsByName = new();

    // References to fields in SaveData, declared for convenience of a shorter name
    private List<string> ghostsInParty;
    private string selectedGhost;

    private void Awake()
    {
        instance = this;

        foreach (GhostIdentity ghost in FindObjectsOfType<GhostIdentity>())
        {
            ghostsByName.Add(ghost.name, ghost);
        }

        SaveManager saveManager = FindObjectOfType<SaveManager>();

        ghostsInParty = saveManager.data.ghostsInParty;
        selectedGhost = saveManager.data.selectedGhost;
    }

    /// <summary>
    /// Adds ghost to end of player's ghost list
    /// </summary>
    /// <param ghostName="ghost"></param>
    public bool TryAddGhostToParty(GhostIdentity ghost)
    {
        if (!ghost.IsInParty() && ghostsInParty.Count < ghostLimit)
        {
            ghostsInParty.Add(ghost.name);
            ghost.SetPartyStatus(true);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Called whenever the hotbar action is triggered by player input,
    /// ignoring `0` value inputs
    /// </summary>
    public void OnHotbar(InputValue value)
    {
        Debug.Log("HOTBAR: " + (int)value.Get<float>());
        int keyValue = (int)value.Get<float>();
        if (keyValue != 0)
        {
            // hotkey #2 is 0th ghost index in list
            ChangePosessingGhost(keyValue - 2);
        }
    }

    /// <summary>
    /// Switches the currently posessing ghost based on hotkey input (1,2,3, etc.)
    /// </summary>
    /// <param name="inputNum">The index to select from the list(value is either 1(player kit), 2, or 3)</param>
    public void ChangePosessingGhost(int index)
    {
        // handle bad input
        if (index >= ghostsInParty.Count)
        {
            return;
        }

        // deselect all ghosts in the list
        for (int i = 0; i < ghostsInParty.Count; i++)
        {
            ghostsByName[ghostsInParty[i]].SetSelected(false);
        }

        // do not possess if player selected base kit
        if (index == -1)
        {
            selectedGhost = null;
            return;
        }

        ghostsByName[ghostsInParty[index]].SetSelected(true);
        selectedGhost = ghostsInParty[index];
    }

    /// <summary>
    /// Removes from player's major ghost list based off reference
    /// </summary>
    /// <param ghostName="ghostIndex"></param>
    public void RemoveGhostFromParty(GhostIdentity ghost)
    {
        ghostsInParty.Remove(ghost.name);
    }

    /// <summary>
    /// Allow external scripts to access player's major ghosts
    /// </summary>
    /// <returns></returns>
    public List<GhostIdentity> GetGhostMajorList()
    {
        Debug.Log(string.Join(", ", ghostsInParty));
        return ghostsInParty.Select(ghostName => ghostsByName[ghostName]).ToList();
    }

    public GhostIdentity GetSelectedGhost()
    {
        return ghostsByName.GetValueOrDefault(selectedGhost);
    }
}
