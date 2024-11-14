using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGhost : MonoBehaviour
{
    [SerializeField] SkillTree skillTree;
    [SerializeField] GhostSkillTreeUIBehaviour ui;

    public void Start()
    {
        ui.Visualize(skillTree);
    }

}
