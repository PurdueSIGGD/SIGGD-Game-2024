using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public static readonly int LEVELS_PER_STEP = 1;
    public static readonly int SKILL_POINT_LIMIT_REG = 3;
    public static readonly int TIER_COUNT = 4;
    public static readonly int TIER_1 = 0;
    public static readonly int TIER_2 = 1;
    public static readonly int TIER_3 = 2;
    public static readonly int TIER_4 = 3;

    [SerializeField]
    private Skill[] skills;

    [SerializeField]
    private int startAtLevel = 0;

    private int[] steps;
    private SkillTier[] skillTiers;
    private int level = 0;

    private void Awake()
    {
        string identityName = gameObject.name;
        if (!SaveManager.data.ghostLevel.ContainsKey(identityName))
        {
            SaveManager.data.ghostLevel.Add(identityName, 10);
        }
        startAtLevel = SaveManager.data.ghostLevel[identityName];

        // initialize the skill tiers
        skillTiers = new SkillTier[TIER_COUNT];
        for (int i = 0; i < skillTiers.Length; i++)
        {
            int lidx = i * 2 + 0;
            int ridx = i * 2 + 1;
            skillTiers[i].leftSkill = (lidx < skills.Length) ? skills[lidx] : null;
            skillTiers[i].rightSkill = (ridx < skills.Length) ? skills[ridx] : null;
            skillTiers[i].unusedPoints = 0;
            skillTiers[i].isUnlocked = false;
        }

        skillTiers[0].isUnlocked = true;

        // intiailize steps for leveling up
        List<int> list = new List<int>();

        // add regular steps
        for (int i = 0; i < SKILL_POINT_LIMIT_REG; i++)
        {
            list.Add(TIER_1);
        }
        for (int i = 0; i < SKILL_POINT_LIMIT_REG; i++)
        {
            list.Add(TIER_2);
        }
        for (int i = 0; i < SKILL_POINT_LIMIT_REG; i++)
        {
            list.Add(TIER_3);
        }

        // add special tier step
        list.Add(TIER_4);

        // add bonus steps
        list.Add(TIER_1);
        list.Add(TIER_2);
        list.Add(TIER_3);

        steps = list.ToArray();
    }

    private void Start()
    {
        for (int i = 0; i < startAtLevel; i++)
        {
            LevelUp();
        }
    }

    private void Update()
    {
        /*int currStep = level / LEVELS_PER_STEP;
        if (currStep < steps.Length)
        {
            // unlock current tier
            skillTiers[steps[currStep]].isUnlocked = true;
        }*/
    }
    
    private int GetSkillTierIndex(Skill skill)
    {
        Debug.Log("skillTiers: " + skillTiers == null);
        for (int i = 0; i < skillTiers.Length; i++)
        {
            if (skillTiers[i].leftSkill == skill || skillTiers[i].rightSkill == skill)
            {
                return i;
            }
        }
        return -1; // should never reach
    }

    // -- External --
    public void LevelUp()
    {
        int currStep = level / LEVELS_PER_STEP;
        int nextStep = (level + 1) / LEVELS_PER_STEP;
        if (currStep < steps.Length)
        {
            if (currStep != nextStep)
            {
                skillTiers[steps[currStep]].unusedPoints++;
                skillTiers[steps[currStep]].isUnlocked = true;

                if (steps[currStep] == TIER_4)
                {
                    skillTiers[steps[currStep]].unusedPoints--;
                    skillTiers[steps[currStep]].leftSkill.AddPoint();
                }
            }
            level++;
        }
        SaveManager.data.ghostLevel[GetComponent<GhostIdentity>().name] = level;
    }

    public void TryAddPoint(Skill skill)
    {
        int tidx = GetSkillTierIndex(skill);

        if (skillTiers[tidx].unusedPoints > 0)
        {
            skillTiers[tidx].unusedPoints--;
            skill.AddPoint();
        }
    }

    public void RemoveSkillPoint(Skill skill)
    {
        int tidx = GetSkillTierIndex(skill);

        Debug.Log("unused points: " + skillTiers[tidx].unusedPoints);

        if (skillTiers[tidx].unusedPoints > 0)
        {
            Debug.Log("Removed points from: " + skillTiers[tidx].leftSkill.name);
            skillTiers[tidx].unusedPoints--;
        }
    }

    public void ResetPoints(int tierIdx)
    {
        skillTiers[tierIdx].unusedPoints = skillTiers[tierIdx].leftSkill.GetPoints() + skillTiers[tierIdx].rightSkill.GetPoints() + skillTiers[tierIdx].unusedPoints;
        skillTiers[tierIdx].leftSkill.ClearPoints();
        skillTiers[tierIdx].rightSkill.ClearPoints();
    }

    public Skill[] GetAllSkills()
    {
        return skills;
    }

    public bool IsUnlocked(Skill skill)
    {
        return skillTiers[GetSkillTierIndex(skill)].isUnlocked;
    }

    public bool IsUnlocked(int tierIdx)
    {
        return skillTiers[tierIdx].isUnlocked;
    }

    public int GetTierPoints(int tierIdx)
    {
        return skillTiers[tierIdx].unusedPoints;
    }

    public int GetLevel()
    {
        return (level + 1);
    }
}

public struct SkillTier
{
    public Skill leftSkill;
    public Skill rightSkill;
    public int unusedPoints;
    public bool isUnlocked;
}
