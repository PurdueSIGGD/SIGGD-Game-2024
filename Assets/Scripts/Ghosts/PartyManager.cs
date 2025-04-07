/// Party management script that tracks every active major ghost
/// Attaches to Player

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PartyManager : MonoBehaviour
{
    public static PartyManager instance;
    [SerializeField]
    private int ghostLimit; // maximum number of ghosts player can wield at one time

    private List<GhostIdentity> ghostsInParty = new List<GhostIdentity>(); // list of each ghost in party
    private GhostIdentity selectedGhost = null;



    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Adds ghost to end of player's ghost list
    /// </summary>
    /// <param ghostName="ghost"></param>
    public bool TryAddGhostToParty(GhostIdentity ghost)
    {
        if (!ghost.IsInParty() && ghostsInParty.Count < ghostLimit)
        {
            ghostsInParty.Add(ghost);
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
            ghostsInParty[i].SetSelected(false);
        }

        // do not possess if player selected base kit
        if (index == -1)
        {
            selectedGhost = null;
            return;
        }

        ghostsInParty[index].SetSelected(true);
        selectedGhost = ghostsInParty[index];
    }

    /// <summary>
    /// Removes from player's major ghost list based off reference
    /// </summary>
    /// <param ghostName="ghostIndex"></param>
    public void RemoveGhostFromParty(GhostIdentity ghost)
    {
        ghostsInParty.Remove(ghost);
    }

    /// <summary>
    /// Allow external scripts to access player's major ghosts
    /// </summary>
    /// <returns></returns>
    public List<GhostIdentity> GetGhostMajorList()
    {
        return ghostsInParty;
    }

    public GhostIdentity GetSelectedGhost()
    {
        return selectedGhost;
    }
}
