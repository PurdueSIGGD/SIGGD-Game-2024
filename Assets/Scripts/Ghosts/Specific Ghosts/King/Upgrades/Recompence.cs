using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recompence : Skill
{
    private KingManager manager;
    private KingBasic basic;
    private static int pointindex;

    void Start()
    {
        AddPoint();
        manager = GetComponent<KingManager>();
        basic = manager.basic;
    }

    public override void AddPointTrigger()
    {
        pointindex = GetPoints();
        if (pointindex > 0)
        {
            manager.recompenceAvaliable = true;
        }
    }

    public override void ClearPointsTrigger()
    {
        pointindex = GetPoints();
        manager.recompenceAvaliable = false;
    }

    public override void RemovePointTrigger()
    {
        pointindex = GetPoints();
        if (pointindex > 0)
        {
            manager.recompenceAvaliable = true;
        }
    }
}
