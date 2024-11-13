/// Party management script that tracks every active major ghost
/// Attaches to Player

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.InputSystem.Controls;

public class PartyManager : MonoBehaviour
{
    [SerializeField]
    private int ghostMajorLimit; // Amount of major ghosts player can wield at one time

    [SerializeField]
    private float range; // Temporary variable to store find ghost range

    // Major Ghosts
    private List<GhostIdentity> ghostMajorList = new List<GhostIdentity>(); // List of each major ghost

    /// <summary>
    /// Temporary function to find and add ghosts nearby to party if space available. Max 1 ghost per function call
    /// </summary>
    public void OnFindGhosts()
    {
        if (ghostMajorList.Count < ghostMajorLimit)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, range, LayerMask.GetMask("Ghosts"));
            foreach (Collider2D collider in colliders)
            {
                GhostIdentity identity = collider.gameObject.GetComponent<GhostIdentity>();
                if (!identity.IsInParty())
                {
                    AddMajorGhost(identity);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Temporary function to remove the least recent ghost added to party
    /// </summary>
    public void OnRemoveGhosts()
    {
        if (ghostMajorList.Count >= 1)
        {
            RemoveMajorGhost(0);
        }
    }

    /// <summary>
    /// Adds onto player's major ghost list based off index
    /// </summary>
    /// <param ghostName="ghost"></param>
    public void AddMajorGhost(GhostIdentity ghost)
    {
        if (!ghost.IsInParty())
        {
            ghostMajorList.Add(ghost);
            ghost.SetInParty(true);
            //ghost.gameObject.GetComponent<SpriteRenderer>().color = inPartyColor;
            Debug.Log("Major ghosts has been updated. It is now: " + ghostMajorList.Count + "/" + ghostMajorLimit);
        }
    }

    /// <summary>
    /// Removes from player's major ghost list based off index
    /// </summary>
    /// <param ghostName="ghostIndex"></param>
    public void RemoveMajorGhost(int ghostIndex)
    {
        if (ghostMajorList[ghostIndex].IsInParty())
        {
            Debug.Log("Removed " + ghostMajorList[ghostIndex].GetName() + ". Exiting party customization...");

            ghostMajorList[ghostIndex].SetInParty(false);
            //ghostMajorList[ghostIndex].gameObject.GetComponent<SpriteRenderer>().color = notInPartyColor;
            ghostMajorList.RemoveAt(ghostIndex);

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
    /// <param name="inputNum">The int value of the input read from the keyboard(value is always 1-6)</param>
    public void ChangePosessingGhost(int inputNum)
    {
        if (inputNum > ghostMajorList.Count)
        {
            print("Input num out of range of Ghost List");
            return;
        }
        int index = inputNum - 1;

        if (ghostMajorList[index].IsPossessing() == false)
        {
            ghostMajorList[index].SetPossessing(true);
            print("Changed posessed to" + ghostMajorList[index].GetName());
        }
        for (int i = 0; i < ghostMajorList.Count; i++)
        {
            if (i == index)
            {
                continue;
            }
            ghostMajorList[i].SetPossessing(false);
        }
    }

    /// <summary>
    /// Allow external scripts to access player's major ghosts
    /// </summary>
    /// <returns></returns>
    public List<GhostIdentity> GetGhostMajorList()
    {
        return ghostMajorList;
    }
}
