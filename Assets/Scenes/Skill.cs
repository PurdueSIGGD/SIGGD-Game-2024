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
    private int cost;
    private bool unlocked; 

    public Skill(string n, int c) {
        name = n;
        cost = c;
        unlocked = false; 
    }

    /// <summary>
    /// set whether the skill is unlocked 
    /// </summary>
    /// <param name="state"> whether the skill is unlocked </param>
    public void SetUnlock(bool state) {
        unlocked = state;
        UnlockEffect();
    }

    /// <summary>
    /// set the unlock effect 
    /// </summary>
    //protected void UnlockEffect() {}
    public Action UnlockEffect { get; set; }

    public string GetName() {
        return name;
    }

    public int GetCost() {
        return cost;
    }
    
    public bool GetUnlocked() {
        return unlocked;
    }
}