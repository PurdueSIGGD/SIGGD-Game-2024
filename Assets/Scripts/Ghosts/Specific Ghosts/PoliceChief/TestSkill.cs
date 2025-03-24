using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill : Skill
{
    public override void AddPoint()
    {
        this.skillPts++;
    }

    public override void ClearPoints()
    {
        this.skillPts = 0;
    }

    public override void RemovePoint()
    {
        this.skillPts--;
    }
}
