#define DEBUG_LOG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Blocking an attack with Retribution grants Akihito
/// damage resistance and a boost to movement speed for a short time after.
/// </summary>
public class RoninsResolve : Skill
{
    private const string RESISTANCE_STAT = "Damage Resistance Percent Int"; // Resistance is percent as an int
    private const string SPEED_STAT = "Max Running Speed";

    private static int pointIndex = 0;

    private SamuraiManager manager;
    private StatManager playerStats;
    private int addedSpeed = 0;

    [SerializeField] private float boostDuration = 5.0f; // boost duration in seconds
    private float boostTimer = 0.0f; // timer for boost

    private void OnEnable()
    {
        GameplayEventHolder.OnAbilityUsed += HandleBoost;
        playerStats = PlayerID.instance.gameObject.GetComponent<StatManager>();
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
    /// Calculate boost percent based on skill points
    /// </summary>
    /// <returns></returns>
    private float CalculatePercent()
    {
        return pointIndex * 0.15f;
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
        float percent = CalculatePercent();
        boostTimer = boostDuration;

        // Speed

        addedSpeed = (int)(10 * percent * playerStats.ComputeValue(SPEED_STAT)); // FIXME
        playerStats.ModifyStat(SPEED_STAT, addedSpeed);

        // Resistance

        playerStats.ModifyStat(RESISTANCE_STAT, (int)(percent * 100));

#if DEBUG_LOG
        Debug.Log("Ronin's Resolve: Boost added! " + percent * 100 + "%");
        Debug.Log("Speed is now " + playerStats.ComputeValue(SPEED_STAT) +
                  ", Resistance is now " + playerStats.ComputeValue(RESISTANCE_STAT) + "%");
#endif
    }

    /// <summary>
    /// Undo boosts if Akihito is deselected or the boost duration runs out
    /// </summary>
    public void RemoveBoosts()
    {
        boostTimer = 0.0f;

        // Speed

        playerStats.ModifyStat(SPEED_STAT, -addedSpeed);
        addedSpeed = 0;

        // Resistance

        playerStats.ModifyStat(RESISTANCE_STAT, -1 * (int)(CalculatePercent() * 100));

#if DEBUG_LOG
        Debug.Log("Ronin's Resolve: Boost removed!");
        Debug.Log("Speed is now " + playerStats.ComputeValue(SPEED_STAT) +
                  ", Resistance is now " + playerStats.ComputeValue(RESISTANCE_STAT) + "%");
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
