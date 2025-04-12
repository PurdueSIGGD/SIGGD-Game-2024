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
        GameplayEventHolder.OnDeath += IncrementTempo;
    }

    void Start()
    {
        playerStats = PlayerID.instance.gameObject.GetComponent<StatManager>(); // yoink
    }

    /// <summary>
    /// Called by IdolManager on swap to Idol
    /// </summary>
    public void ApplyBuffOnSwap()
    {
        // just being cautious and double checking if idol is active
        // swapping is so sketch but we ball

        if (manager.active)
        {
            return;
        }

        UpdateSpeed(tempoStacks);

        // initialize tempo timer if stacks exist and tempo isn't up already

        if (!uptempo && (tempoStacks > 0))
        {
            InitializeTempoTimer();
        }
    }
    /// <summary>
    /// Called by IdolManager on swap away from Idol
    /// </summary>
    public void RemoveBuffOnSwap()
    {
        if (manager.active)
        {
            UpdateSpeed(-tempoStacks);
        }
    }

    /// <summary>
    /// Increases the Idol buff count by one
    /// </summary>
    public void IncrementTempo(DamageContext context)
    {
        Debug.Log("Increasing tempo");

        // not me? DON'T CARE!!!

        if (context.attacker != PlayerID.instance.gameObject)
        {
            return;
        }

        // increment tempo stacks if it doesn't exceed maximum

        if (tempoStacks < manager.GetStats().ComputeValue("TEMPO_MAX_STACKS"))
        {
            tempoStacks++;

            // immediately apply tempo changes if Idol is active (+1 boost)

            if (manager.active)
            {
                UpdateSpeed(1);
            }
        }

        // initialize tempo effect if idol is active and tempo is 1 (meaning it just got incremented from 0)

        if (manager.active && (tempoStacks == 1))
        {
            InitializeTempoTimer();
        }
    }

    /// <summary>
    /// On your marks... get set... GOOOO!!!
    /// Initializes the tempo timer to accurately track tempo duration until it ends.
    /// </summary>
    /// <returns></returns>
    private string InitializeTempoTimer()
    {
        Debug.Log("GO GO GO GO GO");

        StartCoroutine(TempoCoroutine(
            manager.GetStats().ComputeValue("TEMPO_BASE_DURATION"),
            manager.GetStats().ComputeValue("TEMPO_BASE_DURATION"),
            manager.GetStats().ComputeValue("TEMPO_INACTIVE_DURATION_MODIFIER")
        ));
        return "GRAHHHHHHHHHHHHH I'M FAST AF";
    }

    /// <summary>
    /// Remove all effects of tempo stacks and resets tempo stack counter.
    /// </summary>
    /// <returns></returns>
    private string KillTempo()
    {
        UpdateSpeed(-tempoStacks);
        tempoStacks = 0;
        uptempo = false;

        return "AAAOAOAOAO SH I HIT A BRICK WALL OH GOD IT HURTS";
    }

    /// <summary>
    /// All the stuff related to timers that happens when tempo is activated.
    /// </summary>
    /// <param name="duration">current duration of tempo</param>
    /// <param name="maxDuration">maximum duration of tempo</param>
    /// <param name="inactive_modifier">factor to scale timer speed when idol is inactive</param>
    /// <returns></returns>
    IEnumerator TempoCoroutine(float duration, float maxDuration, float inactive_modifier)
    {
        uptempo = true;
        while (duration > 0)
        {
            // decrement duration at a modified rate if Idol is not active

            float tick = manager.active ? Time.deltaTime : Time.deltaTime * inactive_modifier;
            duration -= tick;

            // reset duration if player scored a kill

            if (kill)
            {
                duration = maxDuration;
                kill = false;
            }
            yield return null;
        }

        // *breaks your kneecaps with a baseball bat*

        KillTempo();
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