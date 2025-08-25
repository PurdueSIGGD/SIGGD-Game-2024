#define DEBUG_LOG
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

/// <summary>
/// Old Fling: using Dash restores a part of Radiant Shield's health
/// </summary>
public class OldFling : Skill
{

    private KingManager manager;
    private void Start()
    {
        manager = gameObject.GetComponent<KingManager>();
    }

    /// <summary>
    /// Called by dash script
    /// </summary>
    public void AddExtraHealth()
    {
        identityName = name;

        if (identityName.Contains("(Clone)"))
        {
            identityName = identityName.Replace("(Clone)", "");
        }

        if (GetPoints() <= 0)
        {
            return;
        }

        // In Party?

        if (!PartyManager.instance.IsGhostInParty(identityName))
        {
            return;
        }

        manager.currentShieldHealth += GetPoints() * 10; // add extra health
        manager.currentShieldHealth = Mathf.Min(manager.currentShieldHealth, 
                                                manager.GetStats().ComputeValue("Shield Max Health"));


        // re-enable the basic ability ui icon to notify the player it is usable again
        // I am not sure if shield should be immediately usable, so I will use this threshold stat for now
        if (manager.currentShieldHealth > manager.GetStats().ComputeValue("Shield Health Cooldown Threshold"))
        {
            manager.setBasicCooldown(0);
        }


#if DEBUG_LOG
        Debug.Log("OldFling: Extra Health Gained, Health now: " + manager.currentShieldHealth + " points: " + GetPoints());
#endif

    }

    public override void AddPointTrigger()
    {
    }

    public override void ClearPointsTrigger()
    {
    }

    public override void RemovePointTrigger()
    {
    }
}
