using UnityEngine;

public class GhostIdentity : MonoBehaviour
{
    [SerializeField] private CharacterSO characterInfo;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int exp = 0;

    private IParty[] partyScripts;
    private ISelectable[] possessingScripts;

    private SkillTree skillTree;


    void Awake()
    {
        partyScripts = this.GetComponents<IParty>();
        possessingScripts = this.GetComponents<ISelectable>();
        skillTree = GetComponent<SkillTree>();
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

    public void AddExp(int amount)
    {
        exp += amount;
        while (exp >= GetRequiredExp())
        {
            exp = exp - GetRequiredExp();
            skillTree.LevelUp();
        }
    }

    public void UnlockGhost()
    {
        isUnlocked = true;
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
    }

    public int GetExp()
    {
        return exp;
    }

    public int GetRequiredExp()
    {
        return (100 * skillTree.GetLevel());
    }

}
