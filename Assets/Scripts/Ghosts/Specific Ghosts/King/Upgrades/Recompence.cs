using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recompence : Skill
{
    private KingManager manager;
    private KingBasic basic;
    private int pointIndex;

    [SerializeField]
    List<int> values = new List<int>
    {
        0, 10, 16, 22, 28
    };

    [SerializeField] public float shieldHealthCost;


    void Start()
    {
        manager = GetComponent<KingManager>();
        basic = manager.basic;
        manager.recompenceAvaliable = (pointIndex > 0);
    }

    public float ComputeDamage()
    {
        //return (pointIndex + 1) * (manager.GetStats().ComputeValue("Shield Max Health") - manager.currentShieldHealth);
        return values[pointIndex];
    }

    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
        if (manager != null) manager.recompenceAvaliable = (pointIndex > 0);
    }

    public override void ClearPointsTrigger()
    {
        pointIndex = GetPoints();
        if (manager != null) manager.recompenceAvaliable = (pointIndex > 0);
    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
        if (manager != null) manager.recompenceAvaliable = (pointIndex > 0);
    }
}
