/// Party management script that tracks every active major ghost
/// Attaches to Player

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PartyManager : MonoBehaviour
{
    [SerializeField]
    private int ghostLimit; // maximum number of ghosts player can wield at one time

    // Major Ghosts
    private List<GhostIdentity> ghostsInParty = new List<GhostIdentity>(); // list of each ghost in party

    /// <summary>
    /// Temporary function to remove the least recent ghost added to party
    /// </summary>
    public void OnRemoveGhosts()
    {
        if (ghostsInParty.Count >= 1)
        {
            RemoveGhostFromParty(0);
        }
    }

    /// <summary>
    /// Adds onto player's major ghost list based off index
    /// </summary>
    /// <param ghostName="ghost"></param>
    public void AddGhostToParty(GhostIdentity ghost)
    {
        if (!ghost.IsInParty())
        {
            ghostsInParty.Add(ghost);
            ghost.SetPartyStatus(true);
            //ghost.gameObject.GetComponent<SpriteRenderer>().color = inPartyColor;
            Debug.Log("Major ghosts has been updated. It is now: " + ghostsInParty.Count + "/" + ghostLimit);
        }
    }

    /// <summary>
    /// Removes from player's major ghost list based off index
    /// </summary>
    /// <param ghostName="ghostIndex"></param>
    public void RemoveGhostFromParty(int ghostIndex)
    {
        if (ghostsInParty[ghostIndex].IsInParty())
        {
            Debug.Log("Removed " + ghostsInParty[ghostIndex].GetName() + ". Exiting party customization...");

            ghostsInParty[ghostIndex].SetPartyStatus(false);
            //ghostsInParty[ghostIndex].gameObject.GetComponent<SpriteRenderer>().color = notInPartyColor;
            ghostsInParty.RemoveAt(ghostIndex);

            //toggleParty = false;
        }
    }
    /// <summary>
    /// Called whenever the hotbar action is triggered by player input,
    /// ignoring `0` value inputs
    /// </summary>
    public void OnHotbar(InputValue value)
    {
        int keyValue = (int)value.Get<float>();
        if (keyValue != 0)
        {
            ChangePosessingGhost((int)value.Get<float>());
        }
    }

    /// <summary>
    /// Switches the currently posessing ghost based on hotkey input (1,2,3, etc.)
    /// </summary>
    /// <param name="inputNum">The index to select from the list(value is either 1(player kit), 2, or 3)</param>
    public void ChangePosessingGhost(int inputNum)
    {
        // hotkey #2 is 0th ghost index in list
        int index = inputNum - 2;

        // handle bad input
        if (index > ghostsInParty.Count)
        {
            print("Input " + index + " out of range of list length " + ghostsInParty.Count + ".");
            return;
        }

        // deselect all ghosts in the list
        for (int i = 0; i < ghostsInParty.Count; i++)
        {
            ghostsInParty[i].SetPossessing(false);
        }

        // do not possess if player selected base kit
        if (index == -1)
        {
            print("Posessed by nobody and nospirit.");
            return;
        }

        ghostsInParty[index].SetPossessing(true);
        print("Possessed by... " + ghostsInParty[index].GetName() + "!!!");
    }

    /// <summary>
    /// Removes from player's major ghost list based off reference
    /// </summary>
    /// <param ghostName="ghostIndex"></param>
    public void RemoveGhostFromParty(GhostIdentity ghost)
    {
        int index = ghostsInParty.IndexOf(ghost);
        if (index != -1)
        {
            RemoveGhostFromParty(index);
        }
    }

    /// <summary>
    /// Allow external scripts to access player's major ghosts
    /// </summary>
    /// <returns></returns>
    public List<GhostIdentity> GetGhostMajorList()
    {
        return ghostsInParty;
    }
    /// <summary>
    /// returns the currently isSelected ghost identity script
    /// </summary>
    /// <returns></returns>
    public GhostIdentity GetCurrentGhost()
    {
        for (int i = 0; i < ghostsInParty.Count; i++)
        {
            if (ghostsInParty[i].IsSelected())
            {
                return ghostsInParty[i];
            }
        }
        return null;
    }
}
