using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SkillTreeUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ghostName;
    [SerializeField] TextMeshProUGUI ghostTitle;
    [SerializeField] SkillUI[] skillUis;
    [SerializeField] TierUI[] tierUis;

    private GameObject ghost;
    private SkillTree skillTree;

    // Start is called before the first frame update
    void Start()
    {
        HideSkillTree();
    }

    // Update is called once per frame
    void Update()
    {
        //Visualize(ghost);
    }

    public void Visualize(GameObject ghost)
    {
        this.gameObject.SetActive(true);
        this.ghost = ghost;
        this.skillTree = ghost.GetComponent<SkillTree>();
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

    public void HideSkillTree()
    {
        this.gameObject.SetActive(false);
        ghost = null;
        skillTree = null;
    }

    public void ResetTier1PointsUI()
    {
        skillTree.ResetPoints(SkillTree.TIER_1);
        Visualize(ghost);
    }

    public void ResetTier2PointsUI()
    {
        skillTree.ResetPoints(SkillTree.TIER_2);
        Visualize(ghost);
    }
    public void ResetTier3PointsUI()
    {
        skillTree.ResetPoints(SkillTree.TIER_3);
        Visualize(ghost);
    }

    public void TryAddPointUI(SkillUI skillUI)
    {
        skillTree.TryAddPoint(skillUI.GetAssociatedSkill());
        Visualize(ghost);
    }
}
