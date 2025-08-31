using System.Collections.Generic;
using UnityEngine;

public class GhostIdentity : MonoBehaviour
{
    [SerializeField] private CharacterSO characterInfo;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int exp = 0;
    [SerializeField] private int expPerSpiritGained = 1;
    [SerializeField] private int expPerPinkSpiritGained = 3;
    [SerializeField]
    List<int> levelReqs = new List<int>()
    { 200, 300, 400,
      600, 700, 800,
      1000, 1200, 1400,
      1600, 2000, 2000,
      2000};
    private int curLevel;
    private int curExp;
    private GhostData data;

    private IParty[] partyScripts;
    private ISelectable[] possessingScripts;

    private SkillTree skillTree;


    void Awake()
    {
        partyScripts = this.GetComponents<IParty>();
        possessingScripts = this.GetComponents<ISelectable>();
        skillTree = GetComponent<SkillTree>();
    }

    void Start()
    {
        string identityName = gameObject.name;
        if (identityName.Contains("(Clone)"))
        {
            identityName = identityName.Replace("(Clone)", "");
        }
        switch (identityName)
        {
            case "Eva-Idol":
                data = SaveManager.data.eva;
                break;
            case "North-Police_Chief":
                data = SaveManager.data.north;
                break;
            case "Akihito-Samurai":
                data = SaveManager.data.akihito;
                break;
            case "Yume-Seamstress":
                data = SaveManager.data.yume;
                break;
            case "Silas-PlagueDoc":
                data = SaveManager.data.silas;
                break;
            case "Aegis-King":
                data = SaveManager.data.aegis;
                break;
            default:
                Debug.LogError("Cannot parse ghost's name attmpting to level up: " + identityName);
                return;
        }
        curExp = data.xp;
        AddExp(curExp);
    }

    void OnEnable()
    {
        Spirit.SpiritCollected += GainExp;
    }

    void OnDisable()
    {
        Spirit.SpiritCollected -= GainExp;
    }

    private void GainExp(Spirit.SpiritType type)
    {
        if (type == Spirit.SpiritType.Pink)
        {
            AddExp(expPerPinkSpiritGained);
        }
        else
        {
            AddExp(expPerSpiritGained);
        }
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
        PlayerID.instance.GetComponent<PlayerParticles>().PlayGhostSelected(characterInfo);
        foreach (ISelectable script in possessingScripts)
        {
            script.Select(PlayerID.instance.gameObject);
        }
    }


    public void TriggerDeSelectedBehavior()
    {
        PlayerID.instance.GetComponent<PlayerParticles>().PlayGhostDeselected(characterInfo);
        foreach (ISelectable script in possessingScripts)
        {
            script.DeSelect(PlayerID.instance.gameObject);
        }
    }

    public void AddExp(int amount)
    {
        exp += amount;
        while (exp >= GetRequiredExp() && skillTree.GetLevel() <= levelReqs.Count)
        {
            if (skillTree.GetLevel() == 10 && data.storyProgress < 5) return; // disable unlock sacrifice until max trust
            exp = exp - GetRequiredExp();
            skillTree.LevelUp();
        }
        data.xp = exp;
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
        if (skillTree.GetLevel() >= levelReqs.Count) 
        {
            Debug.LogWarning("Attempting to level up ghost " + name + " to level higher than expected: " + skillTree.GetLevel());
            return levelReqs[^1];
        }
        return levelReqs[skillTree.GetLevel()];
    }

}
