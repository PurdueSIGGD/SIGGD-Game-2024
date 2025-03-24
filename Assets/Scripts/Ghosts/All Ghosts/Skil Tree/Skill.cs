using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Skill
{
    public static readonly int MAX_SKILL_POINTS = 4;

    [SerializeField]
    protected SkillSO skillSO;
    public abstract bool AddPoint();
    public abstract bool RemovePoint();
    public abstract void ClearPoints();

    public abstract void GetPoints();

    public SkillSO GetSkillsSO()
    {
        return skillSO;
    }
}
