using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// a branch that consists of multiple skills 
/// </summary>
public class Branch
{
    private List<Skill> skillList;
    private int nextUnlockIndex; // index of the skill that can be unlocked next
    
    public void AddSkill(Skill skill) {
        skillList.Add(skill);
    }
    
    /// <summary>
    /// unlock the next skill and push nextUnlockIndex to the next skill
    /// <summary>
    public void UnlockNext() {
        skillList[nextUnlockIndex + 1].SetUnlock(true);
        nextUnlockIndex = nextUnlockIndex + 1;
    }

    /// <summary>
    /// return the last unlocked skill 
    /// <summary>
    public Skill GetLastUnlockedSkill() {
        return skillList[nextUnlockIndex - 1];
    }

    /// <summary>
    ///return the first locked skill in the list, which is the skill that can be unlocked next
    /// <summary>
    public Skill GetNextLockedSkill() {
        return skillList[nextUnlockIndex];
    }

    /// <summary>
    /// return the index of the first locked skill in the list, which is the skill that can be unlocked next
    /// <summary>
    public int GetNextLockedSkillIndex() {
        return nextUnlockIndex + 1;
    }
    
    public List<Skill> GetSkillList(){
        return skillList;
    }
}