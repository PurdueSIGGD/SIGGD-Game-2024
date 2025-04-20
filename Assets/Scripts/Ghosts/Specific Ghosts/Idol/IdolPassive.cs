using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Passive for the Idol Ghost, boosts player speed when enemy dies.
/// </summary>
public class IdolPassive : MonoBehaviour
{
    [SerializeField] public int tempoStacks = 0; // number of stacks
    [SerializeField] bool kill; // has the player just scored a kill?
    [SerializeField] bool uptempo; // have the stored tempo stacks been activated?
    [SerializeField] float duration;
    [SerializeField] float tick;
    [SerializeField] public bool active = false;
    List<string> statNames = new()
        {
            "Max Running Speed",
            "Running Accel.",
            "Running Deaccel.",
            "Max Glide Speed",
            "Glide Accel.",
            "Glide Deaccel."
        };

    // Reference to player stats
    private StatManager playerStats;
    [HideInInspector] public IdolManager manager;

    void OnEnable()
    {
        GameplayEventHolder.OnDeath += IdolOnKill;
    }

    void Start()
    {
        playerStats = PlayerID.instance.gameObject.GetComponent<StatManager>(); // yoink
    }

    void Update()
    {
        // tempo duration timer

        if (uptempo && duration > 0)
        {
            float inactiveModifier = manager.GetStats().ComputeValue("TEMPO_INACTIVE_DURATION_MODIFIER");
            float maxDuration = manager.GetStats().ComputeValue("TEMPO_BASE_DURATION");

            // decrement duration at a modified rate if Idol is not active

            tick = active ? Time.deltaTime : Time.deltaTime * inactiveModifier;
            duration -= tick;

            // reset duration if player scored a kill

            if (kill)
            {
                duration = maxDuration;
                kill = false;
            }
        }
        else if (uptempo)
        {
            // end tempo if duration ended

            KillTempo();
        }
    }

    /// <summary>
    /// Called by IdolManager on swap to Idol, AFTER manager.active is set true
    /// </summary>
    public void ApplyBuffOnSwap()
    {
        // just being cautious and double checking if idol is active
        // swapping is so sketch but we ball

        if (active)
        {
            return;
        }
        active = true;
        UpdateSpeed(tempoStacks);
        Debug.Log("TEMPO AWAH UP");

        // initialize tempo timer if stacks exist and tempo isn't up already

        if (!uptempo && (tempoStacks > 0))
        {
            InitializeTempoTimer();
        }
    }
    /// <summary>
    /// Called by IdolManager on swap away from Idol, BEFORE manager.active is set false
    /// </summary>
    public void RemoveBuffOnSwap()
    {
        if (!active)
        {
            return;
        }
        UpdateSpeed(-tempoStacks);
        active = false;
        Debug.Log("TEMPO AWAH DOWN");
    }

    /// <summary>
    /// Increases the Idol buff count by one
    /// </summary>
    public void IdolOnKill(DamageContext context)
    {
        // not me? DON'T CARE!!!

        if (context.attacker != PlayerID.instance.gameObject)
        {
            return;
        }
        kill = true;

        IncrementTempo(1);
    }

    /// <summary>
    /// Increases the Idol buff count by one
    /// </summary>
    public void IncrementTempo(int stacks)
    {
        Debug.Log("Increasing tempo");

        int remainingStacks = (int)manager.GetStats().ComputeValue("TEMPO_MAX_STACKS") - tempoStacks;

        // increment tempo stacks by stacks so it doesn't exceed maximum

        stacks = stacks < remainingStacks ? stacks : remainingStacks;
        tempoStacks += stacks;

        // immediately apply tempo changes if Idol is active (+1 boost)

        if (active)
        {
            UpdateSpeed(stacks);
        }

        // initialize tempo effect if idol is active, has stacks, and isn't speed boosted yet 

        if (active && tempoStacks > 0 && !uptempo)
        {
            InitializeTempoTimer();
        }

        GetComponent<IdolUIDriver>().basicAbilityUIManager.pingAbility();
    }

    /// <summary>
    /// On your marks... get set... GOOOO!!!
    /// Initializes the tempo timer to accurately track tempo duration until it ends.
    /// </summary>
    /// <returns></returns>
    private string InitializeTempoTimer()
    {
        Debug.Log("GO GO GO GO GO");
        duration = manager.GetStats().ComputeValue("TEMPO_BASE_DURATION");
        uptempo = true;
        return "GRAHHHHHHHHHHHHH I'M FAST AF";
    }

    /// <summary>
    /// Remove all effects of tempo stacks and resets tempo stack counter.
    /// </summary>
    /// <returns></returns>
    private string KillTempo()
    {
        if (active)
        {
            UpdateSpeed(-tempoStacks);
        }
        tempoStacks = 0;
        uptempo = false;

        return "AAAOAOAOAO SH I HIT A BRICK WALL OH GOD IT HURTS";
    }

    /// <summary>
    /// Modify player speed stats by changes in player stack count
    /// </summary>
    /// <param name="deltaStack"></param>
    private void UpdateSpeed(int delta)
    {
        // apply changes to each speed stat

        int mod = (int)manager.GetStats().ComputeValue("TEMPO_BUFF_PERCENT_INT");
        foreach (string statName in statNames)
        {
            // flip scaling direction for deacceleration stats

            delta = statName.Contains("Deaccel") ? delta * -1 : delta;
            playerStats.ModifyStat(statName, mod * delta);
        }
        Debug.Log("RUN STAT: " + playerStats.ComputeValue("Max Running Speed"));
    }
}