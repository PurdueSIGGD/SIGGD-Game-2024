using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SkillTreeUI : MonoBehaviour, IScreenUI
{
    // -- Serialize Fields --
    [Header("References")]
    [SerializeField] TextMeshProUGUI ghostName;
    [SerializeField] TextMeshProUGUI ghostTitle;

    // -- Private Variables --
    private GameObject ghost;
    private SkillTree skillTree;
    private SkillUI[] skillUis;
    private TierUI[] tierUis;
    private UnityAction actionOnTreeClose;

    // -- Internal Functions --
    private void Start()
    {
        skillUis = GetComponentsInChildren<SkillUI>();
        tierUis = GetComponentsInChildren<TierUI>();
        HideSkillTree();
    }

    private void Update()
    {
        //Visualize(ghost);
    }

    // -- External Functions --
    public void Visualize(GameObject ghost)
    {
        this.ghost = ghost;
        this.skillTree = ghost.GetComponent<SkillTree>();

        this.gameObject.SetActive(true);
        Skill[] skills = skillTree.GetAllSkills();

        // display each skill (if unlocked)
        for (int i = 0; i < skillUis.Length; i++)
        {
            if (skillTree.IsUnlocked(skills[i]))
            {
                skillUis[i].gameObject.SetActive(true);
                skillUis[i].Visualize(skills[i], this);
            }
            else
            {
                skillUis[i].gameObject.SetActive(false);
            }
        }

        // display each tier's unused points (if unlocked)
        for (int tier = 0; tier < tierUis.Length; tier++)
        {
            if (skillTree.IsUnlocked(tier))
            {
                tierUis[tier].gameObject.SetActive(true);
                tierUis[tier].Visualize(tier, this);
            }
            else
            {
                tierUis[tier].gameObject.SetActive(false);
            }
        }

        PlayerID.instance.FreezePlayer();
    }

    public void HideSkillTree()
    {
        this.gameObject.SetActive(false);
        ghost = null;
        skillTree = null;
        actionOnTreeClose?.Invoke();

        PlayerID.instance.UnfreezePlayer();
    }

    public void ResetTierPointsUI(int tier)
    {
        skillTree.ResetPoints(tier);
        Visualize(ghost);
    }

    public void TryAddPointUI(Skill skill)
    {
        skillTree.TryAddPoint(skill);
        Visualize(ghost);
    }

    public SkillTree GetSkillTree()
    {
        return skillTree;
    }

    public void OnNextCloseCall(UnityAction action)
    {
        actionOnTreeClose = action;
    }
}
