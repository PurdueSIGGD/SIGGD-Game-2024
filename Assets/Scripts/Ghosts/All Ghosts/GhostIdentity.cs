using UnityEngine;

public class GhostIdentity : MonoBehaviour
{
    [SerializeField]
    private string ghostName;

    private bool inParty = false;
    private bool isSelected = false;

    private IParty[] partyScripts;
    private ISelectable[] possessingScripts;
    private int trust;

    void Start()
    {
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

    public void SetPartyStatus(bool inParty)
    {
        foreach (IParty script in partyScripts)
        {
            if (inParty)
            {
                script.EnterParty(PlayerID.instance.gameObject);
            }
            else
            {
                script.ExitParty(PlayerID.instance.gameObject);
            }
        }
        this.inParty = inParty;
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public void SetSelected(bool possessing)
    {
        this.isSelected = possessing;
        foreach (ISelectable action in possessingScripts)
        {
            if (this.isSelected)
            {
                action.Select(PlayerID.instance.gameObject);
            }
            else
            {
                action.DeSelect(PlayerID.instance.gameObject);
            }

        }
    }
    
    public void AddTrust(int amount) {
        trust += amount;
        Debug.Log(trust);
    }
}
