using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch
{
    private List<Skill> skillList;
    private int index;

    public void AddSkill(Skill skill) {
        skillList.Add(skill);
    }
    public void UnlockNext() {
        skillList[index + 1].SetUnlock(true);
        index = index + 1;
    }

    public Skill GetLastUnlockedSkill() {
        return skillList[index];
    }

    public Skill GetNextNextLockedSkill() {
        return skillList[index + 1];
    }

    public int GetNextLockedSkill() {
        return index + 1;
    }
    
    public List<Skill> GetSkillList(){
        return skillList;
    }
}