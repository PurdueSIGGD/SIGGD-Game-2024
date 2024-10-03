using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void SetUnlock(bool state) {
        unlocked = state;
        UnlockEffect();
    }

    public bool GetUnlocked() {
        return unlocked;
    }

    protected abstract void UnlockEffect() {

    }
    public string GetName() {
        return name;
    }

    public int GetCost() {
        return cost;
    }
}
