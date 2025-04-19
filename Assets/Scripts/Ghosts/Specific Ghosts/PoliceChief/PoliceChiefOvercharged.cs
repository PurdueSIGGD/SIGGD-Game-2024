using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceChiefOvercharged : Skill
{
    [SerializeField] int[] pointCounts = {0, 40, 80, 120, 160};
    private int bonusDamage = 0;
    private StatManager statManager;

    private void Start()
    {
    }

    public override void AddPointTrigger()
    {
        bonusDamage = pointCounts[GetPoints()];
        statManager.ModifyStat("Special Damage", bonusDamage - pointCounts[GetPoints() - 1]);
    }
    public override void RemovePointTrigger()
    {

    }
    public override void ClearPointsTrigger()
    {
        statManager.ModifyStat("Special Damage", - bonusDamage);
    }

}
