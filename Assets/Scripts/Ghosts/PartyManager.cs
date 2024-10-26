/// Party management script that tracks every active major ghost
/// Attaches to Player

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PartyManager : MonoBehaviour
{
    // Party Customization
    //[SerializeField]
    //private Color inPartyColor; // Indicate ghosts are in the party
    //[SerializeField]
    //private Color notInPartyColor; // Indicate ghosts are not in the party
    [SerializeField] 
    private int ghostMajorLimit; // Amount of major ghosts player can wield at one time
    //public InputAction partySettings; // Button to start removing ghosts
    //public InputAction chooseMajorGhostRemove; // Select ghost to remove
    //private bool toggleParty; // Toggle to prevent removing same index multiple times
    
    [SerializeField]
    private float range; // Temporary variable to store find ghost range

    // Major Ghosts
    private List<GhostIdentity> ghostMajorList = new List<GhostIdentity>(); // List of each major ghost

    /*void OnEnable() 
    {
        partySettings.Enable();
        chooseMajorGhostRemove.Enable();
    }

    void OnDisable() 
    {
        partySettings.Disable();
        chooseMajorGhostRemove.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        toggleParty = false;
    }*/

    /*// Update is called once per frame
    void Update()
    {
        if (partySettings.triggered) {
            toggleParty = !toggleParty;
            if (toggleParty) {
                Debug.Log("Ready to remove major ghost. Press [B] to cancel.");
            } else {
                Debug.Log("Exiting party customization...");
            }
        }
        if (toggleParty) {
            try { // there is probably a better way to do this
                if (chooseMajorGhostRemove.ReadValue<float>() == -1) {
                    RemoveMajorGhost(0);
                } else if (chooseMajorGhostRemove.ReadValue<float>() == 1) {
                    RemoveMajorGhost(1);
                }
            } catch (System.Exception e) {}
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) 
    {
        GhostIdentity ghost = collision.gameObject.GetComponent<GhostIdentity>();
        if (collision.gameObject.CompareTag("GhostMajor") && ghostMajorList.Count < ghostMajorLimit) {
            AddMajorGhost(ghost);
        }
    }*/


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
                if (!identity.isInParty())
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
        if (!ghost.isInParty()) {
            ghostMajorList.Add(ghost);
            ghost.setInParty(true);
            //ghost.gameObject.GetComponent<SpriteRenderer>().color = inPartyColor;
            Debug.Log("Major ghosts has been updated. It is now: " + ghostMajorList.Count + "/" + ghostMajorLimit);
        }
    }

    /// <summary>
    /// Removes from player's major ghost list based off index
    /// </summary>
    /// <param ghostName="ghostIndex"></param>
    public void RemoveMajorGhost(int ghostIndex) { 
        if (ghostMajorList[ghostIndex].isInParty()) {
            Debug.Log("Removed " + ghostMajorList[ghostIndex].getName() + ". Exiting party customization...");
            
            ghostMajorList[ghostIndex].setInParty(false);
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
