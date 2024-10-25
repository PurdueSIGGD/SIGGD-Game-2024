using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// A skill that can be shown in the UI and has an effect that triggers when skill points are added or removed.
/// /// </summary>
public class Skill
{
    public static readonly int SPECIAL_NUMBER_COUNT = 6;

    protected string name;
    protected string description;
    protected int skillPts;
    protected float[] specialNumbers;
    public Action<float> effect;

    /// <summary>
    /// Constructor for a Skill
    /// </summary>
    /// <param name="name">Name of skill in UI</param>
    /// <param name="description">Description of skill in UI</param>
    /// <param name="specialNums">Numbers for skill's effect and in UI</param>
    public Skill(string name, string description, float[] specialNums) {
        this.name = name;
        this.description = description;
        skillPts = 0;
        this.specialNumbers = specialNums;
    }

    /// <returns>The name of the skill to be shown in UI</returns>
    public string GetName()
    {
        return name;
    }

    /// <summary>
    /// Replaces "XXX" in description with special number
    /// </summary>
    /// <param name="pts">The number of skill points for which description to show</param>
    /// <returns>The description of the skill to be shown in UI given</returns>
    public string GetDescription(int pts)
    {
        String value = "" + specialNumbers[skillPts];
        return description.Replace("XXX", value);
    }

    public int GetSkillPts() {
        return skillPts;
    }

    /// <summary>
    /// Adds skill points to skill, bounded by 0 and 6 (inclusively)
    /// </summary>
    /// <param name="pts">The number of skill points to add</param>
    /// <returns>If points added successfully (result within bounds)</returns>
    public bool AddSkillPts(int pts) {
        int attemptPts = skillPts + pts;
        if (attemptPts >= 0 && attemptPts  <= 6) {
            this.skillPts = attemptPts;
            effect(specialNumbers[skillPts]);
            return true;
        } else
        {
            return false;
        }
    }
}
