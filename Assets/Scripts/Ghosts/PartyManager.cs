/// Party management script that tracks every active major ghost
/// Attaches to Player

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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
    public void AddMajorGhost(GhostIdentity ghost) {
        if (!ghost.IsInParty()) {
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
    public void RemoveMajorGhost(int ghostIndex) { 
        if (ghostMajorList[ghostIndex].IsInParty()) {
            Debug.Log("Removed " + ghostMajorList[ghostIndex].GetName() + ". Exiting party customization...");
            
            ghostMajorList[ghostIndex].SetInParty(false);
            //ghostMajorList[ghostIndex].gameObject.GetComponent<SpriteRenderer>().color = notInPartyColor;
            ghostMajorList.RemoveAt(ghostIndex);

            //toggleParty = false;
        }
    }

    /// <summary>
    /// Allow external scripts to access player's major ghosts
    /// </summary>
    /// <returns></returns>
    public List<GhostIdentity> GetGhostMajorList() {
        return ghostMajorList;
    }
}
