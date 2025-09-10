#define DEBUG_LOG

using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Properties;
using UnityEngine;

/// <summary>
/// Blocking an attack with Retribution grants Akihito
/// damage resistance and a boost to movement speed for a short time after.
/// </summary>
public class RoninsResolve : Skill
{
    private const string RUNNING_SPEED_STAT = "Max Running Speed";
    private const string RUNNING_ACCEL_STAT = "Running Accel.";
    private const string GLIDE_SPEED_STAT = "Max Glide Speed";
    private const string GLIDE_ACCEL_STAT = "Glide Accel.";
    private const string DAMAGE_RESISTANCE_STAT = "Damage Resistance";

    [SerializeField]
    List<int> values = new List<int>
    {
        0, 15, 30, 45, 60
    };
    public int pointIndex = 0;

    private SamuraiManager manager;
    private StatManager playerStats;
    private PlayerHealth playerHealth;

    private int currentStatBoost = 0;
    private bool active = false;



    private void Start()
    {
        playerStats = PlayerID.instance.gameObject.GetComponent<StatManager>();
        playerHealth = PlayerHealth.instance;
        manager = gameObject.GetComponent<SamuraiManager>();
    }

    private void Update()
    {

    }





    /// <summary>
    /// Calculate and add boosts to player
    /// </summary>
    public void AddBoosts(float currentWrath)
    {
        if (pointIndex <= 0) return;
        currentWrath *= 100f;
        float statBoostThresholdValue = 100f / (float) values[pointIndex];
        int deservedStatBoost = Mathf.FloorToInt(currentWrath / statBoostThresholdValue);
        if (deservedStatBoost <= currentStatBoost) return;

        int gainedStatBoost = deservedStatBoost - currentStatBoost;
        currentStatBoost = deservedStatBoost;
        Debug.Log("currentStatBoost now: " + currentStatBoost + "%");

        if (!manager.selected) return;
        BuffStats(gainedStatBoost);

#if DEBUG_LOG
        Debug.Log("Ronin's Resolve: Boost added! " + gainedStatBoost + "%");
        Debug.Log("Running Speed is now " + playerStats.ComputeValue(RUNNING_SPEED_STAT) +
                  ", Resistance is now " + playerHealth.GetDamageResistance() * 100 + "%");
#endif
    }



    public void RemoveBoosts(float currentWrath)
    {
        if (pointIndex <= 0) return;
        currentWrath *= 100f;
        float statBoostThresholdValue = 100f / (float) values[pointIndex];
        int deservedStatBoost = Mathf.FloorToInt(currentWrath / statBoostThresholdValue);
        if (deservedStatBoost >= currentStatBoost) return;

        int lostStatBoost = currentStatBoost - deservedStatBoost;
        currentStatBoost = deservedStatBoost;
        Debug.Log("currentStatBoost now: " +  currentStatBoost + "%");

        if (!manager.selected) return;
        DebuffStats(lostStatBoost);

#if DEBUG_LOG
        Debug.Log("Ronin's Resolve: Boost removed!");
        Debug.Log("Running Speed is now " + playerStats.ComputeValue(RUNNING_SPEED_STAT) +
                  ", Resistance is now " + playerHealth.GetDamageResistance() * 100 + "%");
#endif
    }





    public void ActivateBoosts()
    {
        if (active) return;
        active = true;
        if (currentStatBoost <= 0 || pointIndex <= 0) return;
        BuffStats(currentStatBoost);

#if DEBUG_LOG
        Debug.Log("Ronin's Resolve: Boost added!");
        Debug.Log("SWAP ON!!! Running Speed is now " + playerStats.ComputeValue(RUNNING_SPEED_STAT) +
                  ", Resistance is now " + playerHealth.GetDamageResistance() * 100 + "%");
#endif
    }



    public void DeactivateBoosts()
    {
        if (!active) return;
        active = false;
        if (currentStatBoost <= 0 || pointIndex <= 0) return;
        DebuffStats(currentStatBoost);

#if DEBUG_LOG
        Debug.Log("Ronin's Resolve: Boost removed!");
        Debug.Log("SWAP OFF!!! Running Speed is now " + playerStats.ComputeValue(RUNNING_SPEED_STAT) +
                  ", Resistance is now " + playerHealth.GetDamageResistance() * 100 + "%");
#endif
    }





    private void BuffStats(int buff)
    {
        playerStats.ModifyStat(RUNNING_SPEED_STAT, buff);
        playerStats.ModifyStat(RUNNING_ACCEL_STAT, buff);
        playerStats.ModifyStat(GLIDE_SPEED_STAT, buff);
        playerStats.ModifyStat(GLIDE_ACCEL_STAT, buff);
        //playerHealth.ModifyDamageResistance((float) (buff) / 100f);
        playerStats.ModifyStat(DAMAGE_RESISTANCE_STAT, buff);
    }

    private void DebuffStats(int debuff)
    {
        playerStats.ModifyStat(RUNNING_SPEED_STAT, -debuff);
        playerStats.ModifyStat(RUNNING_ACCEL_STAT, -debuff);
        playerStats.ModifyStat(GLIDE_SPEED_STAT, -debuff);
        playerStats.ModifyStat(GLIDE_ACCEL_STAT, -debuff);
        //playerHealth.ModifyDamageResistance(-((float) (debuff) / 100f));
        playerStats.ModifyStat(DAMAGE_RESISTANCE_STAT, -debuff);
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
