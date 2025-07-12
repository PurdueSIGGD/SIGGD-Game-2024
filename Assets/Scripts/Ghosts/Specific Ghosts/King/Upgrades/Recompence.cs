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
        manager = GetComponent<KingManager>();
        basic = manager.basic;
    }

    public float ComputeDamage()
    {
        return (pointindex + 1) * (manager.GetStats().ComputeValue("Shield Max Health") - manager.currentShieldHealth);
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
