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

    [SerializeField]
    List<int> values = new List<int>
    {
        0, 15, 30, 45, 60
    };
    private int pointIndex = 0;

    private SamuraiManager manager;
    private StatManager playerStats;
    private PlayerHealth playerHealth;

    [SerializeField] public float boostDuration = 5.0f; // boost duration in seconds
    [HideInInspector] public float boostTimer = 0.0f; // timer for boost

    private int currentStatBoost = 0;
    private bool active = false;



    private void Start()
    {
        //GameplayEventHolder.OnAbilityUsed += HandleBoost;
        playerStats = PlayerID.instance.gameObject.GetComponent<StatManager>();
        playerHealth = PlayerHealth.instance;
        manager = gameObject.GetComponent<SamuraiManager>();
    }

    private void OnDisable()
    {
        //GameplayEventHolder.OnAbilityUsed -= HandleBoost;
    }

    private void Update()
    {
        /*
        if (boostTimer > 0.0f) // if boost is activated
        {
            // Check if deselected

            if (!manager.selected)
            {
                RemoveBoosts();
                return;
            }

            // Update timer

            boostTimer -= Time.deltaTime;

            if (boostTimer <= 0.0f)
            {
                RemoveBoosts();
            }
            
        }
        */
    }

    /// <summary>
    /// Calculate boost percent (as integer) based on skill points
    /// </summary>
    /// <returns></returns>
    /*
    private int CalculatePercentInt()
    {
        return pointIndex * 15;
    }
    */

    /// <summary>
    /// When Retribution is used, see whether we should add the speed/resistance boost
    /// </summary>
     
    /*
    private void HandleBoost(ActionContext context)
    {
        if (manager.selected && pointIndex > 0 &&
            context.actionID == ActionID.SAMURAI_SPECIAL &&
            context.extraContext.Equals("Parry Success"))
        {
            if (boostTimer <= 0.0f) // Boost not in effect
            {
                AddBoosts();
            }
            else
            {
                // Boost already in effect. Restart timer.

                boostTimer = boostDuration;
#if DEBUG_LOG
                Debug.Log("Ronin's Resolve: Boost timer restarted!");
#endif
            }

            
        }
    }
    */

    /// <summary>
    /// Calculate and add boosts to player
    /// </summary>
    public void AddBoosts(float currentWrath)
    {
        //float statPercent = values[pointIndex] * (wrathGained / 100f);
        //int percentInt = Mathf.FloorToInt(statPercent);

        //boostTimer = boostDuration;

        if (pointIndex <= 0) return;
        currentWrath *= 100f;
        float statBoostThresholdValue = 100f / (float) values[pointIndex];
        int deservedStatBoost = Mathf.FloorToInt(currentWrath / statBoostThresholdValue);
        if (deservedStatBoost <= currentStatBoost) return;

        int gainedStatBoost = deservedStatBoost - currentStatBoost;
        currentStatBoost = deservedStatBoost;
        Debug.Log("currentStatBoost now: " + currentStatBoost + "%");

        if (!manager.selected) return;
        //playerStats.ModifyStat(RUNNING_SPEED_STAT, gainedStatBoost);
        //playerHealth.ModifyDamageResistance((float) (gainedStatBoost) / 100f);
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
        //playerStats.ModifyStat(RUNNING_SPEED_STAT, -lostStatBoost);
        //playerHealth.ModifyDamageResistance(-((float) (lostStatBoost) / 100f));
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
        //playerStats.ModifyStat(RUNNING_SPEED_STAT, currentStatBoost);
        //playerHealth.ModifyDamageResistance((float) (currentStatBoost) / 100f);
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
        //playerStats.ModifyStat(RUNNING_SPEED_STAT, -currentStatBoost);
        //playerHealth.ModifyDamageResistance(-((float) (currentStatBoost) / 100f));
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
        playerHealth.ModifyDamageResistance((float) (buff) / 100f);
    }

    private void DebuffStats(int debuff)
    {
        playerStats.ModifyStat(RUNNING_SPEED_STAT, -debuff);
        playerStats.ModifyStat(RUNNING_ACCEL_STAT, -debuff);
        playerStats.ModifyStat(GLIDE_SPEED_STAT, -debuff);
        playerStats.ModifyStat(GLIDE_ACCEL_STAT, -debuff);
        playerHealth.ModifyDamageResistance(-((float) (debuff) / 100f));
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
