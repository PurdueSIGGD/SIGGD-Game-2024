//#define DEBUG_LOG

using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/// <summary>
/// Blocking an attack with Retribution grants Akihito
/// damage resistance and a boost to movement speed for a short time after.
/// </summary>
public class RoninsResolve : Skill
{
    private const string RUNNING_SPEED_STAT = "Max Running Speed";
    //private const string MOVE_SPEED_STAT = "Move Speed";

    [SerializeField]
    List<float> values = new List<float>
    {
        0f, 15f, 30f, 45f, 60f
    };
    private int pointIndex = 0;

    private SamuraiManager manager;
    private StatManager playerStats;
    private PlayerHealth playerHealth;

    [SerializeField] public float boostDuration = 5.0f; // boost duration in seconds
    [HideInInspector] public float boostTimer = 0.0f; // timer for boost

    private void OnEnable()
    {
        GameplayEventHolder.OnAbilityUsed += HandleBoost;
        playerStats = PlayerID.instance.gameObject.GetComponent<StatManager>();
        playerHealth = PlayerHealth.instance;
        manager = gameObject.GetComponent<SamuraiManager>();
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnAbilityUsed -= HandleBoost;
    }

    private void Update()
    {
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
    }

    /// <summary>
    /// Calculate boost percent (as integer) based on skill points
    /// </summary>
    /// <returns></returns>
    private int CalculatePercentInt()
    {
        return pointIndex * 15;
    }

    /// <summary>
    /// When Retribution is used, see whether we should add the speed/resistance boost
    /// </summary>
     
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

    /// <summary>
    /// Calculate and add boosts to player
    /// </summary>
    public void AddBoosts()
    {
        int percentInt = Mathf.CeilToInt(values[pointIndex]);
        boostTimer = boostDuration;

        // Speed

        playerStats.ModifyStat(RUNNING_SPEED_STAT, percentInt);
        //playerStats.ModifyStat(MOVE_SPEED_STAT, percentInt);

        // Resistance

        playerHealth.ModifyDamageResistance(percentInt / 100.0f);

#if DEBUG_LOG
        Debug.Log("Ronin's Resolve: Boost added! " + percentInt + "%");
        Debug.Log("Running Speed is now " + playerStats.ComputeValue(RUNNING_SPEED_STAT) +
                  ", Move Speed is now " + playerStats.ComputeValue(MOVE_SPEED_STAT) +
                  ", Resistance is now " + playerHealth.GetDamageResistance() * 100 + "%");
#endif
    }

    /// <summary>
    /// Undo boosts if Akihito is deselected or the boost duration runs out
    /// </summary>
    public void RemoveBoosts()
    {
        int percentInt = Mathf.CeilToInt(values[pointIndex]);
        boostTimer = 0.0f;

        // Speed

        playerStats.ModifyStat(RUNNING_SPEED_STAT, -percentInt);
        //playerStats.ModifyStat(MOVE_SPEED_STAT, -percentInt);

        // Resistance

        playerHealth.ModifyDamageResistance(-(percentInt / 100.0f));

#if DEBUG_LOG
        Debug.Log("Ronin's Resolve: Boost removed!");
        Debug.Log("Running Speed is now " + playerStats.ComputeValue(RUNNING_SPEED_STAT) +
                  ", Move Speed is now " + playerStats.ComputeValue(MOVE_SPEED_STAT) +
                  ", Resistance is now " + playerHealth.GetDamageResistance() * 100 + "%");
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
