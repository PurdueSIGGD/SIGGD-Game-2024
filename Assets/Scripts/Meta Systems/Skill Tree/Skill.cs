using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Skill : MonoBehaviour
{
    [SerializeField]
    protected SkillSO skillSO;

    protected int skillPts = 0;

    public void AddPoint()
    {
        skillPts++;
        AddPointTrigger();
    }
    public void RemovePoint()
    {
        skillPts--;
        RemovePointTrigger();
    }
    public void ClearPoints()
    {
        skillPts = 0;
        ClearPointsTrigger();
    }

    public abstract void AddPointTrigger();
    public abstract void RemovePointTrigger();
    public abstract void ClearPointsTrigger();

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

    public Sprite GetIcon()
    {
        return skillSO.sprite;
    }

    public string GetDescriptionValue()
    {
        return skillSO.descriptionValue;
    }
}
