using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SkillTreeUI : MonoBehaviour
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
            } else
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
            } else
            {
                tierUis[tier].gameObject.SetActive(false);
            }
        }
    }

    public void HideSkillTree()
    {
        this.gameObject.SetActive(false);
        ghost = null;
        skillTree = null;
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
}
