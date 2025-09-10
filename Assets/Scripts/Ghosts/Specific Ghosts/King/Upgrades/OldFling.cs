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

    [SerializeField]
    List<float> values = new List<float>
    {
        0, 10, 20, 30, 40
    };
    public int pointIndex;

    [SerializeField] private GameObject pulseVFX;

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

        if (pointIndex <= 0)
        {
            return;
        }

        // In Party?

        if (!PartyManager.instance.IsGhostInParty(identityName))
        {
            return;
        }

        if (manager.currentShieldHealth >= manager.GetStats().ComputeValue("Shield Max Health")) return;

        // VFX & SFX
        GameObject pulse = Instantiate(pulseVFX, PlayerID.instance.transform);
        pulse.GetComponent<RingExplosionHandler>().playRingExplosion(2.5f, GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
        AudioManager.Instance.SFXBranch.PlaySFXTrack("Aegis-Shield On Damage");


        manager.currentShieldHealth += values[pointIndex]; // add extra health
        manager.currentShieldHealth = Mathf.Min(manager.currentShieldHealth, 
                                                manager.GetStats().ComputeValue("Shield Max Health"));


        // re-enable the basic ability ui icon to notify the player it is usable again
        // I am not sure if shield should be immediately usable, so I will use this threshold stat for now
        /*
        if (manager.currentShieldHealth > manager.GetStats().ComputeValue("Shield Health Cooldown Threshold"))
        {
            manager.setBasicCooldown(0);
        }
        */

        if (manager.getBasicCooldown() > 0f)
        {
            float cooldownReduction = manager.GetStats().ComputeValue("Basic Cooldown") * (values[pointIndex] / manager.GetStats().ComputeValue("Shield Max Health"));
            manager.setBasicCooldown(manager.getBasicCooldown() - cooldownReduction);
        }


#if DEBUG_LOG
        Debug.Log("OldFling: Extra Health Gained, Health now: " + manager.currentShieldHealth + " points: " + GetPoints());
#endif

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
