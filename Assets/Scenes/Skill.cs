using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// a skill that makes an effect when unlocked. The unlock state is false as default
/// </summary>
public class Skill
{
    private string name;
    private int skillPts;
    private bool unlocked; 
    private float[] specialNumbers;

    public Skill(string n, int pts, float[] speNums) {
        name = n;
        skillPts = pts;
        unlocked = false; 
        specialNumbers = speNums;
    }

    public Skill(string n, float[] speNums) {
        name = n;
        skillPts = 0;
        unlocked = false;
        specialNumbers = speNums;
    }

    /// <summary>
    /// set whether the skill is unlocked 
    /// </summary>
    /// <param name="state"> whether the skill is unlocked </param>
    public void SetUnlock(bool state) {
        unlocked = state;
        DoEffect();
    }

    /// <summary>
    /// set the unlock effect 
    /// </summary>
    //protected void UnlockEffect() {}
    public Action<float> effect;

    public void DoEffect() {
        effect(specialNumbers[skillPts]);
    }

    public string GetName() {
        return name;
    }

    public int GetSkillPts() {
        return skillPts;
    }

    public void AddSkillPts(int pts) {
        this.skillPts += pts;
        DoEffect();
    }
    
    public bool GetUnlocked() {
        return unlocked;
    }
}
