using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sacrifice : Skill
{
    private int pointIndex;

    public virtual void DoSac()
    {
        if (GetPoints() <= 0) return;
        Health playerHealth = PlayerID.instance.GetComponent<Health>();
        playerHealth.currentHealth = Mathf.Min((playerHealth.currentHealth + (playerHealth.GetStats().ComputeValue("Max Health") * playerHealth.GetStats().ComputeValue("Mortal Wound Threshold"))),
                                               playerHealth.GetStats().ComputeValue("Max Health"));

        PartyManager.instance.SwitchGhostToIndex(-1);
        PartyManager.instance.RemoveGhostFromParty(GetComponent<GhostIdentity>());
    }

    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void ClearPointsTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
    }
}
