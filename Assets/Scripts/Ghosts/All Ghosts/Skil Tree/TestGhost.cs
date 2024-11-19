using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGhost : MonoBehaviour
{
    [SerializeField] SkillTree skillTree;
    [SerializeField] GhostSkillTreeUIBehaviour ui;

    public void Start()
    {
        skillTree.tierList[0].Unlock();
        skillTree.tierList[0].AddSkillPts();
        skillTree.tierList[1].Unlock();
        skillTree.tierList[2].Unlock();
        ui.Visualize(skillTree);
    }

}
