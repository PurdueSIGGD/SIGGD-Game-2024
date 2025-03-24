using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Skill : MonoBehaviour
{
    public static readonly int MAX_SKILL_POINTS = 4;

    [SerializeField]
    protected SkillSO skillSO;

    protected int skillPts;

    public abstract void AddPoint();
    public abstract void RemovePoint();
    public abstract void ClearPoints();

    public int GetPoints()
    {
        return skillPts;
    }

    public string GetName()
    {
        return skillSO.name;
    }

    public string GetDescription()
    {
        return skillSO.description;
    }
}
