using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeOne : BasicSkillTree
{
    public SkillTreeOne() : base(50) {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Branch br1 = new Branch();
        Branch br2 = new Branch();
        Branch br3 = new Branch();
        br1.AddSkill(new Skill("Fireball")
        {
            UnlockEffect = ()=> {
                Debug.Log("Levek 1 Skill 1");
            }
        });
        br1.AddSkill(new Skill("FireWall")
        {
            UnlockEffect = ()=> {
                Debug.Log("Level 1 Skill 2: ");
            }
        });
        br2.AddSkill(new Skill("FireSth1")
        {
            UnlockEffect =()=> {
                Debug.Log("Level 2 skill 1");
            }
        });
        br2.AddSkill(new Skill("FireSth2")
        {
            UnlockEffect =()=> {
                Debug.Log("Level 2 skill 2");
            }
        });
        base.AddBranch(br1);
        base.AddBranch(br2);
        br1.AddSkillPts();
        br1.AddSkillPts();
        br2.AddSkillPts();
        br1.SwapSkillPts(0, 1);

        foreach(Branch b in this.branchList) {
            foreach (Skill s in b.GetSkillList()) {
                s.UnlockEffect();
                Debug.Log("name: " + s.GetName() + "; Skill Points: " + s.GetSkillPts().ToString());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
