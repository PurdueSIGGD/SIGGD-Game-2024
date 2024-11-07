using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeOne : BasicSkillTree
{
    private SkillTier st1 = new SkillTier();
    private SkillTier st2 = new SkillTier();
    private SkillTier st3 = new SkillTier();
    private FinalSkill final;
    public SkillTreeOne() : base(0) {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        float[] nums = {0.0f, 5.0f, 10.0f, 15.0f, 20.0f, 23.0f, 25.0f};
        st1.SetSkillOne(new Skill("Fireball", nums) {
            effect = number => {
                gameObject.GetComponent<Renderer>().material.color = new Color(0.5f * number, 1.0f * number, 1.5f * number);
                Debug.Log("Level 1 Skill 1");
                Debug.Log("Special numer value is: " + number);
            }
        });
        st1.SetSkillTwo(new Skill("FireWall", nums) {
            effect = number => {
                transform.position = new Vector3(number, number, number);
                Debug.Log("Level 1 Skill 2");
                Debug.Log("Special numer value is: " + number);
            }
        });
        st2.SetSkillOne(new Skill("FireSth1", nums) {
            effect = number => {
                Debug.Log("Level 2 Skill 1");
            }
        });
        st2.SetSkillTwo(new Skill("FireSth2", nums) {
            effect = number => {
                Debug.Log("Level 2 Skill 2");
            }
        });
        st3.SetSkillOne(new Skill("a ghostName", nums) {
            effect = number => {
                Debug.Log("Level 3 Skill 1");
            }
        });
        st3.SetSkillTwo(new Skill("another ghostName", nums) {
            effect = number => {
                Debug.Log("Level 3 Skill 2");
            }
        });
        final = new FinalSkill("final", nums) {
            effect = number => {
                Debug.Log("Level 4");
            }
        };

        base.tierList[0] = st1;
        base.tierList[1] = st2;
        base.tierList[2] = st3;
        base.SetFinalSkill(final);
        /*
        st1.AddSkillPts();
        st1.AddSkillPts();
        st1.AddSkillPts();
        st1.SwapSkillPts(0, 1);*/
        st2.AddSkillPts();
    }

    bool preA = false;
    bool preS = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && preA == false)
        {
            st1.AddSkillPts();
            preA = true;
        }
        if (!Input.GetKeyDown(KeyCode.A)) {
            preA = false;
        }
        /*
        if (Input.GetKeyDown(KeyCode.S) && preS == false)
        {
            st2.AddSkillPts();
            preS = true;
        }
        if (!Input.GetKeyDown(KeyCode.S)) {
            preS = false;
        }*/
    }
}
