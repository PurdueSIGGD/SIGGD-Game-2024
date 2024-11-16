using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SpiritPartyManager : MonoBehaviour {
    [SerializeField] 
    private int spiritLimit; // Amount of major ghosts player can wield at one time
    
    [SerializeField]
    private float range; // Temporary variable to store find ghost range

    // Major Ghosts
    private List<GhostIdentity> spiritList = new List<GhostIdentity>(); // List of each major ghost

    /// <summary>
    /// Temporary function to find and add ghosts nearby to party if space available. Max 1 ghost per function call
    /// </summary>
    public void OnFindGhosts()
    {
        if (spiritList.Count < spiritLimit)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, range, LayerMask.GetMask("Ghosts"));
            foreach (Collider2D collider in colliders)
            {
                GhostIdentity identity = collider.gameObject.GetComponent<GhostIdentity>();
                if (!identity.IsInParty())
                {
                    AddSpirit(identity);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Temporary function to remove the least recent ghost added to party
    /// </summary>
    public void OnRemoveSpirits()
    {
        if (spiritList.Count >= 1)
        {
            RemoveSpirit(0);
        }
    }

    /// <summary>
    /// Adds onto player's major ghost list based off index
    /// </summary>
    /// <param ghostName="ghost"></param>
    public void AddSpirit(GhostIdentity ghost) {
        if (!ghost.IsInParty()) {
            spiritList.Add(ghost);
            ghost.SetInParty(true);
            //ghost.gameObject.GetComponent<SpriteRenderer>().color = inPartyColor;
            Debug.Log("Major ghosts has been updated. It is now: " + spiritList.Count + "/" + spiritLimit);
        }
    }

    /// <summary>
    /// Removes from player's major ghost list based off index
    /// </summary>
    /// <param ghostName="ghostIndex"></param>
    public void RemoveSpirit(int ghostIndex) { 
        if (spiritList[ghostIndex].IsInParty()) {
            Debug.Log("Removed " + spiritList[ghostIndex].GetName() + ". Exiting party customization...");
            
            spiritList[ghostIndex].SetInParty(false);
            //ghostMajorList[ghostIndex].gameObject.GetComponent<SpriteRenderer>().color = notInPartyColor;
            spiritList.RemoveAt(ghostIndex);

            //toggleParty = false;
        }
    }

    /// <summary>
    /// Allow external scripts to access player's major ghosts
    /// </summary>
    /// <returns></returns>
    public List<GhostIdentity> GetGhostMajorList() {
        return spiritList;
    }
}
