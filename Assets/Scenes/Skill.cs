using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// a skill that makes an effect when unlocked. Its unlock state is set false as default
/// </summary>
public abstract class Skill
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
    
    public bool GetUnlocked() {
        return unlocked;
    }

    /// <summary>
    /// set the unlock effect 
    /// </summary>
    protected abstract void UnlockEffect();
    
    public string GetName() {
        return name;
    }

    public int GetCost() {
        return cost;
    }
}
