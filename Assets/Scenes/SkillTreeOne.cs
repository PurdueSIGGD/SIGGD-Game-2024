using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillTreeOne : BasicSkillTree
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        Branch br1 = new Branch();
        Skill temp = new Skill("Fireball", 30)
        {
            UnlockEffect = ()=> {
                Debug.Log("Hi!");
            }
        };
        br1.AddSkill(temp);
        br1.AddSkill(new Skill("FireWall", 5)
        {
            UnlockEffect = ()=> {
                Debug.Log("World!");
            }
        });
        Branch br2 = new Branch();
        
        base.AddBranch(br1);
        //AddBranch(br2);
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(this.branchList[0].GetSkillList().Count);
        foreach (Skill s in this.branchList[0].GetSkillList()) {
            Debug.Log("hello");
            s.UnlockEffect();
        }
    }
}
