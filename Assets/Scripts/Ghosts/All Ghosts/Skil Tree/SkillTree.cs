using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public static readonly int LEVELS_PER_STEP = 2;
    public static readonly int SKILL_POINT_LIMIT_REG = 3;

    [SerializeField]
    private Skill[] skills;

    private SkillTier[] steps;

    private int[] tierPoints = new int[3];
    [SerializeField] private int level = 0;

    private void Awake()
    {
        List<SkillTier> list = new List<SkillTier>();

        // add regular steps
        for (int i = 0; i < SKILL_POINT_LIMIT_REG; i++)
        {
            list.Add(SkillTier.TIER_1);
        }
        for (int i = 0; i < SKILL_POINT_LIMIT_REG; i++)
        {
            list.Add(SkillTier.TIER_2);
        }
        for (int i = 0; i < SKILL_POINT_LIMIT_REG; i++)
        {
            list.Add(SkillTier.TIER_3);
        }

        // add special tier
        list.Add(SkillTier.TIER_4);

        // add bonus steps
        list.Add(SkillTier.TIER_1);
        list.Add(SkillTier.TIER_2);
        list.Add(SkillTier.TIER_3);

        steps = list.ToArray();
    }

    void Start()
    {
    }

    void Update()
    {
        
    }

    // -- External --
    public void LevelUp()
    {
        int preStep = level / LEVELS_PER_STEP;
        int postStep = (level + 1) / LEVELS_PER_STEP;
        if (preStep < steps.Length)
        {
            if (preStep != postStep)
            {
                SkillTier tier = steps[preStep];
                if (tier != SkillTier.TIER_4)
                {
                    tierPoints[(int) tier]++;
                } else
                {
                    skills[skills.Length - 1].AddPoint();
                }
            }
        }
        level++;
        Debug.Log($"Level {level} \t PreStep {preStep} \t PostStep {postStep}");
    }

    public void TryAddPoint(Skill skill)
    {
        int tierIdx = Array.IndexOf(skills, skill) / 2;

        if (tierPoints[tierIdx] > 0)
        {
            tierPoints[tierIdx] = tierPoints[tierIdx] - 1;
            skill.AddPoint();
        }        
    }

    public void ResetPoints(SkillTier tier)
    {
        int tidx = (int) tier;
        int lidx = tidx * 2;
        int ridx = tidx * 2 + 1;

        tierPoints[tidx] = skills[lidx].GetPoints() + skills[ridx].GetPoints() + tierPoints[tidx];

        skills[lidx].ClearPoints();
        skills[ridx].ClearPoints();
    }

    public Skill[] GetAllSkills()
    {
        return skills;
    }

    public bool IsUnlocked(Skill skill)
    {
        int idx = Array.IndexOf(skills, skill);
        int points = 0;
        if (idx % 2 == 0)
        {
            points = tierPoints[idx / 2] + skill.GetPoints() + skills[idx + 1].GetPoints();
        } 
        else
        {
            points = tierPoints[idx / 2] + skill.GetPoints() + skills[idx - 1].GetPoints();
        }

        return (points > 0);
    }

    public int GetTierPoints(SkillTier tier)
    {
        return tierPoints[(int) tier];
    }

    public void OnTestA()
    {
        LevelUp();
    }

}

public enum SkillTier
{
    TIER_1 = 0,
    TIER_2 = 1,
    TIER_3 = 2,
    TIER_4 = 3
}
