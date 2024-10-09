using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGhostTree : BasicSkillTree
{
    // Start is called before the first frame update
    void Start()
    {
        Branch br1 = new Branch();
        br1.AddSkill(new Skill (
            @Override
            public void UnlockEffect() {
                this.getPlayer().addStaticeffect(Movement, 0.5%)

            }
        ));

        br1.AddSkill(new Skill (
            @Override
            public void UnlockEffect() {
                this.getPlayer().addStaticeffect(Movement, 0.5%)

            }
        ));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
