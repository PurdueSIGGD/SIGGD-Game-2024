using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillTreeUI : MonoBehaviour
{
    [SerializeField] public SkillUI[] skillUis;
    [SerializeField] public TextMeshProUGUI[] tierUis;
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

        for (int i = 0; i < skillUis.Length; i++)
        {
            skillUis[i].Visualize(skills[i]);
        }

        for (int i = 0; i < tierUis.Length; i++)
        {
            tierUis[i].SetText(skillTree.GetTierPoints((SkillTier) i).ToString());
        }
    }

    public void ResetPointsUITier1()
    {
        skillTree.ResetPoints(SkillTier.TIER_1);
        Visualize(skillTree);
    }

    public void ResetPointsUITier2()
    {
        skillTree.ResetPoints(SkillTier.TIER_2);
        Visualize(skillTree);
    }
    public void ResetPointsUITier3()
    {
        skillTree.ResetPoints(SkillTier.TIER_3);
        Visualize(skillTree);
    }

    public void TryAddPointUI(SkillUI skillUI)
    {
        skillTree.TryAddPoint(skillUI.GetAssociatedSkill());
        Visualize(skillTree);
    }

    /*public Skill AddSkillPointToSkill(Skill skill, int points)
    {
        for (int i = 0; i < skillUis.Length; i++)
        {
            if (i % 2 == 0)
            {
                if(this.skillTree.tierList[i / 2].GetLeftSkill() == skill)
                {
                    if (this.skillTree.tierList[i/2].GetTotalSkillPts() >= points)
                    {
                        this.skillTree.tierList[i / 2].GetLeftSkill().AddSkillPts(points);
                        this.skillTree.tierList[i / 2].SubSkillPts();
                        return this.skillTree.tierList[i / 2].GetLeftSkill();
                    }
                }
            }
            else
            {
                if (this.skillTree.tierList[i / 2].GetRightSkill() == skill)
                {
                    if (this.skillTree.tierList[i / 2].GetTotalSkillPts() >= points)
                    {
                        this.skillTree.tierList[i / 2].GetRightSkill().AddSkillPts(points);
                        this.skillTree.tierList[i / 2].SubSkillPts();
                        return this.skillTree.tierList[i / 2].GetRightSkill();
                    }
                }
            }
        }
        return null;
    }*/
}
