using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSkillTreeUIBehaviour : MonoBehaviour
{
    [SerializeField] public GhostSkillUIBehaviour[] skillsUIs;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Visualize(SkillTree skillTree)
    {
        for(int i = 0; i < skillsUIs.Length; i++){
            if(i % 2 == 0)
            {
                skillsUIs[i].Visualize(skillTree.tierList[i / 2].GetLeftSkill());
            }
            else{
                skillsUIs[i].Visualize(skillTree.tierList[i / 2].GetRightSkill());
            }
        }
    }
}
