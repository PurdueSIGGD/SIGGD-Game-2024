using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public static readonly int MAX_LEVEL = 10;
    public static readonly int SKILL_POINT_LIMIT_REG = 3;
    public static readonly int SKILL_POINT_LIMIT_MAX = 4;


    [SerializeField]
    private Skill[] skills;

    [SerializeField] private int[] tierPoints = new int[3];
    [SerializeField] private int level = 0;

    void Start()
    {
    }

    void Update()
    {
        
    }

    // -- External --
    public void LevelUp()
    {
        if (level + 1 < MAX_LEVEL)
        {
            int tier = level / 3;
            tierPoints[tier] = tierPoints[tier] + 1;
            level++;
        } else if (level + 1 == MAX_LEVEL)
        {
            skills[skills.Length - 1].AddPoint();
        }
    }
    
    public void TryAddPoint(Skill skill)
    {
        int tierIdx = Array.IndexOf(skills, skill) / 2;
        int limit = (level == MAX_LEVEL) ? SKILL_POINT_LIMIT_MAX : SKILL_POINT_LIMIT_REG;

        if (tierPoints[tierIdx] > 0 && skill.GetPoints() < limit)
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
        /*Skill[] unlocked = new Skill[((level / 3) + 1) * 2];
        for (int i = 0; i < unlocked.Length; i++)
        {
            unlocked[i] = skills[i];
        }
        return unlocked;*/
        return skills;
    }

    public bool IsUnlocked(Skill skill)
    {
        int check = ((level / 3) + 1) * 2;
        return (Array.IndexOf(skills, skill) < check);
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
    TIER_3 = 2
}
