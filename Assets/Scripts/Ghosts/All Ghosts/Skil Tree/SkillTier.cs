using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A tier that consists of a left skill and a right skill and can be unlocked.
/// </summary>

[System.Serializable]
public class SkillTier
{
    [SerializeField] public Skill skillLeft;
    [SerializeField] public Skill skillRight;
    private bool unlocked = false;
    private int totalSkillPts = 0;
    
    public static readonly int SKILL_LEFT = 0;
    public static readonly int SKILL_RIGHT = 1;

    public SkillTier(Skill skillOne, Skill skillTwo)
    {
        this.skillLeft = skillOne;
        this.skillRight = skillTwo;
    }
    
    /// <returns>The Left Skill (intended for UI purposes only!)</returns>
    public Skill GetLeftSkill()
    {
        return this.skillLeft;
    }

    /// <returns>The Right Skill (intended for UI purposes only!)</returns>
    public Skill GetRightSkill()
    {
        return this.skillRight;
    }


    /// <summary>
    /// Unlocks skill tier. Once unlocked, cannot be locked again.
    /// </summary>
    public void Unlock() {
        unlocked = true; 
    }

    /// <summary>
    /// Adds a skill point to skill tier only if skill tier is already unlocked.
    /// Skill point is automatically added to the first skill.
    /// <summary>
    public void AddSkillPts() {
        if (unlocked)
        {
            totalSkillPts += 1;
            skillLeft.AddSkillPts(1);
        } 
    }

    /// <summary>
    /// Swaps 1 skill point from one skill to another. Use SkillTier.SKILL_LEFT and SkillTier.SKILL_RIGHT for arguments.
    /// </summary>
    /// <param name="skill">SKILL_LEFT = transfer points to left skill, SKILL_RIGHT = transfer points to right skill</param>
    /// <param name="pts"></param>
    /// <returns>If skill point swapped successfully </returns>
    public bool SwapSkillPtsTo(int skill) {
        if (skill == SKILL_RIGHT) { 
            if (skillLeft.GetSkillPts() >= 1) {
                skillLeft.AddSkillPts(-1);
                skillRight.AddSkillPts(1);
                return true;
            } else
            {
                Debug.Log("Not enough skill pts");
                return false;
            }
        } else {
            if (skillRight.GetSkillPts() >= 1) {
                skillRight.AddSkillPts(-1);
                skillLeft.AddSkillPts(1);
                return true;
            } else
            {
                Debug.Log("Not enough skill pts");
                return false;
            }
        }
    }    
}