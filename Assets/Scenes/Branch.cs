using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a tier that consists maximum of two skills 
/// </summary>
public class Branch
{
    private List<Skill> skillList = new List<Skill>();
    private bool unlock = false;
    private int totalSkillPts = 0;
    
    protected void Start()
    {

    }

    public void AddSkill(Skill skill) {
        this.skillList.Add(skill);
    }

    public void SetUnlock(bool state) {
        unlock = state; 
        foreach(Skill s in this.skillList) {
            s.SetUnlock(true);
        }
    }

    /// <summary>
    /// add a skill point to the trust level. It should be automatically added to the first skill 
    /// <summary>
    public void AddSkillPts() {
        totalSkillPts += 1;
        this.skillList[0].AddSkillPts(1);
    }

    public void SwapSkillPts(int taken, int given) {
        if (this.skillList[taken].GetSkillPts() >= 1) {
            this.skillList[taken].AddSkillPts(-1);
            this.skillList[given].AddSkillPts(1);
        } else {
            Debug.Log("Not Enough skill point");
        }
    }
    
    public List<Skill> GetSkillList(){
        return skillList;
    }
}