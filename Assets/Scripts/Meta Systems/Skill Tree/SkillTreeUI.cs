using UnityEngine;
using UnityEngine.Events;

public class SkillTreeUI : MonoBehaviour, IScreenUI
{
    // ==============================
    //       Serialized Fields
    // ==============================

    [Header("References")]
    [SerializeField] SkillUI[] skillUis;
    [SerializeField] TierUI[] tierUis;

    // ==============================
    //        Other Variables
    // ==============================

    private GameObject ghost;
    private SkillTree skillTree;
    private UnityAction actionOnTreeClose;

    // ==============================
    //        Unity Functions
    // ==============================

    private void Start()
    {
        CloseSkillTree();
    }

    // ==============================
    //       Private Functions
    // ==============================


    // ==============================
    //        Other Functions
    // ==============================

    /// <summary>
    /// Opens and shows the Skill Tree UI for the given ghost game object
    /// </summary>
    public void OpenSkillTree(GameObject ghost)
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
                skillUis[i].Visualize(skillTree, skills[i]);
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
                tierUis[tier].Visualize(skillTree, tier);
            }
            else
            {
                tierUis[tier].gameObject.SetActive(false);
            }
        }

        PlayerID.instance.FreezePlayer();
    }

    /// <summary>
    /// Closes and hides the Skill Tree UI
    /// </summary>
    public void CloseSkillTree()
    {
        this.gameObject.SetActive(false);
        ghost = null;
        skillTree = null;
        actionOnTreeClose?.Invoke();

        PlayerID.instance.UnfreezePlayer();
    }

    /// <summary>
    /// Calls given action the next time skill tree UI is closed
    /// </summary>
    public void OnNextCloseCall(UnityAction action)
    {
        actionOnTreeClose = action;
    }
}
