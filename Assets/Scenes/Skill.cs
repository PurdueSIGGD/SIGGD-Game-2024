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

    public Skill(string n, int pts) {
        name = n;
        skillPts = pts;
        unlocked = false; 
    }

    public Skill(string n) {
        name = n;
        skillPts = 0;
        unlocked = false;
    }

    /// <summary>
    /// set whether the skill is unlocked 
    /// </summary>
    /// <param name="state"> whether the skill is unlocked </param>
    public void SetUnlock(bool state) {
        unlocked = state;
        Effect1();
    }

    /// <summary>
    /// set the unlock effect 
    /// </summary>
    //protected void UnlockEffect() {}
    public Action Effect1 { get; set; }

    public string GetName() {
        return name;
    }

    public int GetSkillPts() {
        return skillPts;
    }

    public void AddSkillPts(int pts) {
        this.skillPts += pts;
    }
    
    public bool GetUnlocked() {
        return unlocked;
    }
}
