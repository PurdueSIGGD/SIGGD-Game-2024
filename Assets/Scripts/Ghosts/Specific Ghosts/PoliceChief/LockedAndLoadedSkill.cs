using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedAndLoadedSkill : Skill
{
    int[] reserveCharges = {0, 3, 6, 9, 12};
    private int pointIndex;
    PoliceChiefSpecial policeChiefSpecial;

    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void ClearPointsTrigger()
    {
    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
    }

    // Start is called before the first frame update
    void Start()
    {
        AddPoint();
        policeChiefSpecial.OnAbilityUsed
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void UsedReserved()
    {
    }
}
