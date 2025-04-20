using UnityEngine;

public class GhostIdentity : MonoBehaviour
{
    [SerializeField] private CharacterSO characterInfo;
    [SerializeField] private bool isUnlocked;

    private IParty[] partyScripts;
    private ISelectable[] possessingScripts;
    private SkillTree skillTree;
    private int trust = 0;
    private int exp = 0;

    void Start()
    {
        partyScripts = this.GetComponents<IParty>();
        possessingScripts = this.GetComponents<ISelectable>();
    }

    /// <summary>
    /// getter method for the ghost's character info
    /// </summary>
    /// <returns>ghost's character info</returns>
    public CharacterSO GetCharacterInfo()
    {
        return characterInfo;
    }

    public void TriggerEnterPartyBehavior()
    {
        foreach (IParty script in partyScripts)
        {
            script.EnterParty(PlayerID.instance.gameObject);
        }
    }

    public void TriggerExitPartyBehavior()
    {
        foreach (IParty script in partyScripts)
        {
            script.ExitParty(PlayerID.instance.gameObject);
        }
    }

    public bool IsSelected()
    {
        return (this == PartyManager.instance.GetSelectedGhost());
    }

    public void TriggerSelectedBehavior()
    {
        foreach (ISelectable script in possessingScripts)
        {
            script.Select(PlayerID.instance.gameObject);
        }
    }


    public void TriggerDeSelectedBehavior()
    {
        foreach (ISelectable script in possessingScripts)
        {
            script.DeSelect(PlayerID.instance.gameObject);
        }
    }

    public void AddTrust(int amount)
    {
        trust += amount;
        Debug.Log(trust);
    }

    public void UnlockGhost()
    {
        isUnlocked = true;
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
    }
}
