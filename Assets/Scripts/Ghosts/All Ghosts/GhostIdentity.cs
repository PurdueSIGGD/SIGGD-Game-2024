using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GhostIdentity : MonoBehaviour, IInteractable
{
    [SerializeField]
    private string ghostName;

    private bool inParty = false;
    private bool possessing = false;
    private GameObject player;
    private IParty[] partyScripts;
    private ISelectable[] possessingScripts;
    private int trust;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        partyScripts = this.GetComponents<IParty>();
        possessingScripts = this.GetComponents<ISelectable>();
    }
    /// <summary>
    /// getter method for the ghost's name
    /// </summary>
    /// <returns>ghost's name</returns>
    public string GetName()
    {
        return ghostName;
    }
    /// <summary>
    /// checks if ghost is in the player's party
    /// </summary>
    /// <returns>if ghost is in party</returns>
    public bool IsInParty()
    {
        return inParty;
    }

    public void SetInParty(bool inParty)
    {
        this.inParty = inParty;
        if (inParty)
        {
            foreach (IParty script in partyScripts)
            {
                script.EnterParty(player);
            }
        }
        else
        {
            foreach (IParty script in partyScripts)
            {
                script.ExitParty(player);
            }
        }
    }

    public bool IsPossessing()
    {
        return possessing;
    }

    public void SetPossessing(bool possessing)
    {
        this.possessing = possessing;
        foreach (ISelectable action in possessingScripts)
        {
            if (this.possessing)
            {
                action.Select(player);
            }
            else
            {
                action.DeSelect(player);
            }

        }
    }

    public void Interact()
    {
        Debug.Log("Ghost Interacted with!");
        Debug.Log(PlayerID.instance == null);
        PlayerID.instance.GetComponent<PartyManager>().AddMajorGhost(this);
    }

    public void AddTrust(int amount) {
        trust += amount;
        Debug.Log(trust);
    }

}
