using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill : Skill
{
    private int skillpts = 0;
    public override bool AddPoint()
    {
        skillpts++;
        return true;
    }

    public override void ClearPoints()
    {
        skillpts = 0;
    }

    public override int GetPoints()
    {
        return skillpts;
    }

    public override bool RemovePoint()
    {
        skillpts--;
        return true;
    }
}
