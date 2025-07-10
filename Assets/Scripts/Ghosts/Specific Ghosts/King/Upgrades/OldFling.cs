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

    private KingManager manager = null;

    private void OnEnable()
    {
        manager = GetComponent<KingManager>();
    }

    /// <summary>
    /// Called by dash script
    /// </summary>
    public void AddExtraHealth()
    {
        if (GetPoints() <= 0)
        {
            return;
        }

        manager.currentShieldHealth += GetPoints() * 10; // add extra health
        manager.currentShieldHealth = Mathf.Min(manager.currentShieldHealth, 
                                                manager.GetStats().ComputeValue("Shield Max Health"));


#if DEBUG_LOG
        Debug.Log("OldFling: Extra Health Gained, Health: " + manager.currentShieldHealth);
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
