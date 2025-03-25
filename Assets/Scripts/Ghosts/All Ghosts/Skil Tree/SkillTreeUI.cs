using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SkillTreeUI : MonoBehaviour
{
    [SerializeField] public SkillUI[] skillUis;
    [SerializeField] public TierUI[] tierUis;
    [SerializeField] SkillTree skillTree;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Visualize(skillTree);
    }

    public void Visualize(SkillTree skillTree)
    {
        this.skillTree = skillTree;
        Skill[] skills = skillTree.GetAllSkills();

        // display each skill (if unlocked)
        for (int i = 0; i < skillUis.Length; i++)
        {
            if (skillTree.IsUnlocked(skills[i]))
            {
                skillUis[i].gameObject.SetActive(true);
                skillUis[i].Visualize(skills[i]);
            }
            else
            {
                skillUis[i].gameObject.SetActive(false);
            }
        }

        // display tier unused points (if unlocked)
        for (int i = 0; i < tierUis.Length; i++)
        {
            if (skillTree.IsUnlocked(i))
            {
                tierUis[i].gameObject.SetActive(true);
                tierUis[i].Visualize(skillTree.GetTierPoints(i));
            } else
            {
                tierUis[i].gameObject.SetActive(false);
            }
        }
    }

    public void ResetTier1PointsUI()
    {
        skillTree.ResetPoints(SkillTree.TIER_1);
        Visualize(skillTree);
    }

    public void ResetTier2PointsUI()
    {
        skillTree.ResetPoints(SkillTree.TIER_2);
        Visualize(skillTree);
    }
    public void ResetTier3PointsUI()
    {
        skillTree.ResetPoints(SkillTree.TIER_3);
        Visualize(skillTree);
    }

    public void TryAddPointUI(SkillUI skillUI)
    {
        skillTree.TryAddPoint(skillUI.GetAssociatedSkill());
        Visualize(skillTree);
    }
}
