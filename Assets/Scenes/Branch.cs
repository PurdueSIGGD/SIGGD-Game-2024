using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Branch
{
    private List<Skill> skillList;
    private int nextUnlockIndex; // index of the skill that can be unlocked next
    
    public void AddSkill(Skill skill) {
        skillList.Add(skill);
    }
    
    //unlock the next skill and push nextUnlockIndex to the next skill
    public void UnlockNext() {
        skillList[nextUnlockIndex + 1].SetUnlock(true);
        nextUnlockIndex = nextUnlockIndex + 1;
    }

    //return the last unlocked skill 
    public Skill GetLastUnlockedSkill() {
        return skillList[nextUnlockIndex]-1;
    }

    //return the first locked skill in the list, which is the skill that can be unlocked next
    public Skill GetNextLockedSkill() {
        return skillList[nextUnlockIndex];
    }

    //return the index of the first locked skill in the list, which is the skill that can be unlocked next
    public int GetNextLockedSkillIndex() {
        return index + 1;
    }
    
    public List<Skill> GetSkillList(){
        return skillList;
    }
}