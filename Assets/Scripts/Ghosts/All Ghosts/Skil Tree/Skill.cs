using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
/// <summary>
/// A skill that can be shown in the UI and has an effect that triggers when skill points are added or removed.
/// /// </summary>
public class Skill
{
    public static readonly int SPECIAL_NUMBER_COUNT = 6;

    [SerializeField] SkillsSO skillSO;
    protected int skillPts;
    protected Action<float> effect;

    /// <summary>
    /// Constructor for a Skill
    /// </summary>
    /// <param name="skill">The Scriptable Object you want this skill to use</param>
    public Skill(SkillsSO skill) {
        skillSO = skill;
        skillPts = 0;
    }

    /// <returns>The name of the skill to be shown in UI</returns>
    public string GetName()
    {
        return skillSO.skillName;
    }

    /// <summary>
    /// Replaces "XXX" in description with special number
    /// </summary>
    /// <returns>The description of the skill to be shown in UI given</returns>
    public string GetDescription()
    {
        String value = "" + skillSO.specialNumbers[skillPts];
        return skillSO.description.Replace("XXX", value);
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
            effect?.Invoke(skillSO.specialNumbers[skillPts]);
            return true;
        } else
        {
            return false;
        }
    }

    public Sprite GetSprite()
    {
        return skillSO.sprite;
    }
}
