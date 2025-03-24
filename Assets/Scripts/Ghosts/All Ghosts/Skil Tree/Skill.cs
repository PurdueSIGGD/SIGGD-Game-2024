using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface Skill
{
    public static readonly int MAX_SKILL_POINTS = 4;

    public bool AddPoint();
    public bool RemovePoint();
    public void ClearPoints();
    public SkillsSO GetSkillsSO();
}
