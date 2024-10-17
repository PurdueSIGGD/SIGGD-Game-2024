using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeOne : BasicSkillTree
{
    private Branch br1 = new Branch();
    private Branch br2 = new Branch();
    private Branch br3 = new Branch();
    private Branch br4 = new Branch();
    public SkillTreeOne() : base(0) {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        float[] nums = {0.0f, 5.0f, 10.0f, 15.0f, 20.0f, 23.0f, 25.0f};
        br1.AddSkill(new Skill("Fireball", nums) {
            effect = number => {
                gameObject.GetComponent<Renderer>().material.color = new Color(1 * number, 2 * number, 3 * number);
                Debug.Log("Level 1 Skill 1");
                Debug.Log("Special numer value is: " + number);
            }
        });
        br1.AddSkill(new Skill("FireWall", nums) {
            effect = number => {
                Debug.Log("Level 1 Skill 2");
                Debug.Log("Special numer value is: " + number);
            }
        });
        br2.AddSkill(new Skill("FireSth1", nums) {
            effect = number => {
                Debug.Log("Level 2 Skill 1");
            }
        });
        br2.AddSkill(new Skill("FireSth2", nums) {
            effect = number => {
                Debug.Log("Level 2 Skill 2");
            }
        });
        br3.AddSkill(new Skill("a name", nums) {
            effect = number => {
                Debug.Log("Level 3 Skill 1");
            }
        });
        br3.AddSkill(new Skill("another name", nums) {
            effect = number => {
                Debug.Log("Level 3 Skill 2");
            }
        });
        br4.AddSkill(new Skill("final", nums) {
            effect = number => {
                Debug.Log("Level 4");
            }
        });

        base.branchList[0] = br1;
        base.branchList[1] = br2;
        base.branchList[2] = br3;
        base.branchList[3] = br4;
        /*
        br1.AddSkillPts();
        br1.AddSkillPts();
        br1.AddSkillPts();
        br1.SwapSkillPts(0, 1);*/
        br2.AddSkillPts();
    }

    bool preA = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && preA == false)
        {
            br1.AddSkillPts();
            preA = true;
        }
        if (!Input.GetKeyDown(KeyCode.A)) {
            preA = false;
        }
    }
}
