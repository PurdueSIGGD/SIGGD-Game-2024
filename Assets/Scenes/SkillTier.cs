using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a tier that consists maximum of two skills 
/// </summary>
public class SkillTier
{
    private List<Skill> skillList = new List<Skill>();
    private Skill skillOne;
    private Skill skillTwo;
    private bool unlock = false;
    private int totalSkillPts = 0;
    
    protected void Start()
    {

    }

    public void SetSkillOne(Skill skill) {
        skillOne = skill;
    }

    public void SetSkillTwo(Skill skill) {
        skillTwo = skill;
    }

    public void SetUnlock(bool state) {
        unlock = state; 
        skillOne.SetUnlock(true);
        skillTwo.SetUnlock(true);
    }

    /// <summary>
    /// add a skill point to the trust level. It should be automatically added to the first skill 
    /// <summary>
    public void AddSkillPts() {
        totalSkillPts += 1;
        skillOne.AddSkillPts(1);
    }

    public void SwapSkillPts(int taken, int given) {
        if (taken == 1) { 
            if (skillOne.GetSkillPts() > 1) {
                skillOne.AddSkillPts(-1);
                skillTwo.AddSkillPts(1);
            } else Debug.Log("Not enough skill pts");
        } else if (taken == 2) {
            if (skillTwo.GetSkillPts() > 1) {
                skillTwo.AddSkillPts(-1);
                skillOne.AddSkillPts(1);
            }
        }
    }
    
    public Skill GetSkillOne() {
        return skillOne;
    }

    public Skill GetSkillTwo() {
        return skillTwo;
    }
}