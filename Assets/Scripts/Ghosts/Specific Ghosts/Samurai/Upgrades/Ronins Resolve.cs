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

    private static int pointIndex = 0;

    private SamuraiManager manager;
    private StatManager playerStats;
    private int addedSpeed = 0;

    [SerializeField] private float boostDuration = 5.0f; // boost duration in seconds
    private float boostTimer = 0.0f; // timer for boost

    void Start()
    {
        playerStats = PlayerID.instance.gameObject.GetComponent<StatManager>();
    }

    private void OnEnable()
    {
        GameplayEventHolder.OnAbilityUsed += HandleBoost;
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
    /// When Retribution is used, see whether we should add the speed/resistance boost
    /// </summary>
     
    private void HandleBoost(ActionContext context)
    {
        Debug.Log(pointIndex + " " + context.extraContext.ToString());
        if (manager.selected && pointIndex > 0 &&
            //context.actionID == ActionID.SAMURAI_SPECIAL &&
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
        boostTimer = boostDuration;
#if DEBUG_LOG
        Debug.Log("Ronin's Resolve: Boost added!");
#endif
    }

    /// <summary>
    /// Undo boosts if Akihito is deselected or the boost duration runs out
    /// </summary>
    public void RemoveBoosts()
    {
        boostTimer = 0.0f;
#if DEBUG_LOG
        Debug.Log("Ronin's Resolve: Boost removed!");
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
