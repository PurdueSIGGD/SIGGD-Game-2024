using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Passive for the Idol Ghost, boosts player speed when enemy dies.
/// </summary>
/// TODO hook up enemy death event to call IncrementTempo
public class IdolPassive : MonoBehaviour
{
    [SerializeField] public int tempoStack = 0; // Buff count, maxes at 3 stacks
    [SerializeField] public int runSpeedMod = 20; // Run speed modifier for each buff stack, in percentage
    [SerializeField] public int glideSpeedMod = 8; // Glide speed modifier for each buff stack, in percentage
    [SerializeField] public float tempoDuration = 8.0f; // Duration before 1 stack of tempo expires

    // Reference to player stats
    private StatManager stats;
    private int maxRunningSpeedIdx;
    private int runningAccelIdx;
    private int runningDeaccelIdx;

    private int maxGlideSpeedIdx;
    private int glideAccelIdx;
    private int glideDeaccelIdx;

    void OnEnable()
    {
        GameplayEventHolder.OnDeath += IncrementTempo;
    }

    void Start()
    {
        stats = GetComponent<StatManager>();
    }

    /// <summary>
    /// Increases the Idol buff count by one
    /// </summary>
    public void IncrementTempo(DamageContext context)
    {
        Debug.Log("Increasing tempo");
        if (context.attacker == gameObject)
        {
            StartCoroutine(TempoCoroutine());
        }
    }

    /// <summary>
    /// Increases the Idol buff count (max 3)
    /// </summary>
    /// <param name="count">number of buff stacks to add</param>
    public void IncrementTempo(int count)
    {
        for (int i = 0; i < count; i++)
        {
            StartCoroutine(TempoCoroutine());
        }
    }

    /// <summary>
    /// Remove all buff stacks.
    /// Please use when switching ghost
    /// </summary>
    public void ResetTempo()
    {
        StopAllCoroutines();
        tempoStack = 0;
    }

    /// <summary>
    /// Modify player speed stats by changes in player stack count
    /// </summary>
    /// <param name="deltaStack"></param>
    private void UpdateSpeed(int deltaStack)
    {
        stats.ModifyStat("Max Running Speed", runSpeedMod * deltaStack);
        stats.ModifyStat("Running Accel.", runSpeedMod * deltaStack);
        stats.ModifyStat("Running Deaccel.", runSpeedMod * deltaStack);

        stats.ModifyStat("Max Glide Speed", glideSpeedMod * deltaStack);
        stats.ModifyStat("Glide Accel.", glideSpeedMod * deltaStack);
        stats.ModifyStat("Glide Deaccel.", glideSpeedMod * deltaStack);
    }

    /// <summary>
    /// Gain 1 stack of tempo
    /// </summary>
    /// <returns></returns>
    private IEnumerator TempoCoroutine()
    {
        if (tempoStack == 3)
        {
            yield break;
        }
        tempoStack ++;
        UpdateSpeed(1);

        yield return new WaitForSeconds(tempoDuration);
        tempoStack --;
        Debug.Log("Tempo expired");
        UpdateSpeed(-1);
    }
}
