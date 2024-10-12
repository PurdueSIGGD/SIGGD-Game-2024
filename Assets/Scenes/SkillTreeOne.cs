using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeOne : BasicSkillTree
{
    public SkillTreeOne() : base(0) {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Branch br1 = new Branch();
        Branch br2 = new Branch();
        Branch br3 = new Branch();
        Branch br4 = new Branch();
        br1.AddSkill(new Skill("Fireball") {
            Effect1 = ()=> {
                gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 255);
                Debug.Log("Level 1 Skill 1");
            }
        });
        br1.AddSkill(new Skill("FireWall") {
            Effect1 = ()=> {
                Debug.Log("Level 1 Skill 2");
            }
        });
        br2.AddSkill(new Skill("FireSth1") {
            Effect1 =()=> {
                Debug.Log("Level 2 Skill 1");
            }
        });
        br2.AddSkill(new Skill("FireSth2") {
            Effect1 =()=> {
                Debug.Log("Level 2 Skill 2");
            }
        });
        br3.AddSkill(new Skill("a name") {
            Effect1 =()=> {
                Debug.Log("Level 3 Skill 1");
            }
        });
        br3.AddSkill(new Skill("another name") {
            Effect1 =()=> {
                Debug.Log("Level 3 Skill 2");
            }
        });
        br4.AddSkill(new Skill("final") {
            Effect1 =()=> {
                Debug.Log("Level 4");
            }
        });
        base.branchList[0] = br1;
        base.branchList[1] = br2;
        base.branchList[2] = br3;
        base.branchList[3] = br4;
        br1.AddSkillPts();
        br1.AddSkillPts();
        br1.AddSkillPts();
        br2.AddSkillPts();
        br1.SwapSkillPts(0, 1);

        /*
        foreach(Branch b in this.branchList) {
            foreach (Skill s in b.GetSkillList()) {
                s.Effect1();
                Debug.Log("name: " + s.GetName() + "; Skill Points: " + s.GetSkillPts().ToString());
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        //base.UpdateProgression();
    }
}
