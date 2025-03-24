using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GhostSkillTreeUIBehaviour : MonoBehaviour
{
    [SerializeField] public GhostSkillUIBehaviour[] skillsUIs;
    [SerializeField] public GameObject[] skillPairs;

    /*private SkillTree skillTree;

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
        this.skillTree = skillTree;
        for(int i = 0; i < skillsUIs.Length; i++){
            if(i % 2 == 0)
            {
                skillsUIs[i].Visualize(skillTree.tierList[i / 2].GetLeftSkill());
            }
            else{
                skillsUIs[i].Visualize(skillTree.tierList[i / 2].GetRightSkill());
            }
        }

        for (int j = 0; j < skillTree.tierList.Length; j++)
        {
            if (skillTree.tierList[j].IsUnlocked())
            {
                skillPairs[j].SetActive(true);
            }
        }
    }

    public Skill AddSkillPointToSkill(Skill skill, int points)
    {
        for (int i = 0; i < skillsUIs.Length; i++)
        {
            if (i % 2 == 0)
            {
                if(this.skillTree.tierList[i / 2].GetLeftSkill() == skill)
                {
                    if (this.skillTree.tierList[i/2].GetTotalSkillPts() >= points)
                    {
                        this.skillTree.tierList[i / 2].GetLeftSkill().AddSkillPts(points);
                        this.skillTree.tierList[i / 2].SubSkillPts();
                        return this.skillTree.tierList[i / 2].GetLeftSkill();
                    }
                }
            }
            else
            {
                if (this.skillTree.tierList[i / 2].GetRightSkill() == skill)
                {
                    if (this.skillTree.tierList[i / 2].GetTotalSkillPts() >= points)
                    {
                        this.skillTree.tierList[i / 2].GetRightSkill().AddSkillPts(points);
                        this.skillTree.tierList[i / 2].SubSkillPts();
                        return this.skillTree.tierList[i / 2].GetRightSkill();
                    }
                }
            }
        }
        return null;
    }*/
}
