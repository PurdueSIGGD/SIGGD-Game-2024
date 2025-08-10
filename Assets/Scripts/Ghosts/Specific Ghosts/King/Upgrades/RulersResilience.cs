#define DEBUG_LOG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Radiant Shield gains extra health, and it does not incur a cooldown time until this extra health is depleted.
/// Bonus Shield Health: +10 | +20 | +30 | +40
/// </summary>
public class RulersResilience : Skill
{

    private const string SHIELD_MAX_HEALTH = "Shield Max Health";

    private KingManager manager;

    private KingManager GetManager()
    {
        if (manager == null)
        {
            manager = gameObject.GetComponent<KingManager>();
        }

        return manager;
    }

    public int GetExtraShieldHealth()
    {

        if (GetPoints() <= 0) return 0;

        int extraHealth = GetPoints() * 10;

        return extraHealth;
    }


    public override void AddPointTrigger() {
        // In Party?

        if (GetManager() == null || GetManager().GetStats() == null)
        {
            return;
        }
        manager.currentShieldHealth = GetManager().GetStats().ComputeValue(SHIELD_MAX_HEALTH) + GetExtraShieldHealth();
    }
    public override void ClearPointsTrigger() {
        // In Party?

        if (GetManager() == null || GetManager().GetStats() == null)
        {
            return;
        }
        manager.currentShieldHealth = GetManager().GetStats().ComputeValue(SHIELD_MAX_HEALTH) + GetExtraShieldHealth();
    }
    public override void RemovePointTrigger() {
        // In Party?

        if (GetManager() == null || GetManager().GetStats() == null)
        {
            return;
        }
        manager.currentShieldHealth = GetManager().GetStats().ComputeValue(SHIELD_MAX_HEALTH) + GetExtraShieldHealth();
    }

}
