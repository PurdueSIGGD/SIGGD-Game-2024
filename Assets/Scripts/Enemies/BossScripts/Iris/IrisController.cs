using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrisController : BossController
{


    [Header("IRIS VARIABLES")]
    [SerializeField] bool startSpawn;
    [SerializeField] IrisVisualsManager visualManager;
    [SerializeField] List<float> damageStateThresholds = new List<float>();
    int damageState;

    [Header("Shield parameters")]
    bool shieldOn;
    [SerializeField] float shieldBreakTimeSeconds;
    [SerializeField] int numWavesToBreakShield;
    int waveCountMirror = 0;
    int wavesSinceShieldUp = 0;
    [SerializeField] int numEnemiesToBreakShield;
    int enemyCountMirror = 0;
    int enemiesSinceShieldUp = 0;

    [Header("Death Implementation parameters")]
    [SerializeField] float deathTimeSeconds;

    public new void Start()
    {
        base.Start();
        ActivateShield();
    }
    public new void Update()
    {
        base.Update();
        if (startSpawn)
        {
            StartWaveSpawning();
            StartPassiveSpawning();
            startSpawn = false;
        }

        // run this code block only on the frame that # of waves is updated
        if (waveCountMirror != GetNumWaves())
        {
            waveCountMirror = GetNumWaves();
            wavesSinceShieldUp++;
            if (shieldOn && (wavesSinceShieldUp > numWavesToBreakShield))
            {
                StartShieldBreakSequence();
            }
        }
        // run this code block only on the frame that # of enemies is updated
        if (enemyCountMirror != GetNumEnemiesKilled())
        {
            enemyCountMirror = GetNumEnemiesKilled();
            enemiesSinceShieldUp++;
            if (shieldOn && (enemiesSinceShieldUp > numEnemiesToBreakShield))
            {
                StartShieldBreakSequence();
            }
        }

        // calculate damage state and adjust visuals accordingly
        float healthProportion = bossHealth.currentHealth / bossHealth.GetStats().ComputeValue("Max Health");
        if (healthProportion > damageStateThresholds[IrisVisualStates.NORMAL])
        {
            damageState = IrisVisualStates.NORMAL;
        }
        else if (healthProportion > damageStateThresholds[IrisVisualStates.DAMAGE_LOW])
        {
            damageState = IrisVisualStates.DAMAGE_LOW;
        }
        else if (healthProportion < damageStateThresholds[IrisVisualStates.DAMAGE_LOW])
        {
            damageState = IrisVisualStates.DAMAGE_HIGH;
        }
        visualManager.SetVisualState(damageState);
    }

    public void ActivateShield()
    {
        ToggleShield(true);
    }
    public void DeactivateShield()
    {
        ToggleShield(false);
    }
    void ToggleShield(bool val)
    {
        shieldOn = val;
        visualManager.ToggleShieldVisual(val);
        if (val)
            EnableInvincibility();
        else
            DisableInvincibility();
    }
    void StartShieldBreakSequence()
    {
        StartCoroutine(IrisShieldCoroutine());
    }
    IEnumerator IrisShieldCoroutine()
    {
        DeactivateShield();
        yield return new WaitForSeconds(shieldBreakTimeSeconds);
        wavesSinceShieldUp = 0;
        enemiesSinceShieldUp = 0;
        if (damageState != IrisVisualStates.DAMAGE_HIGH)
            ActivateShield();
    }
    public override void StartDefeatSequence()
    {
        base.StartDefeatSequence();
        visualManager.ActivateDeathVisual();
        StartCoroutine(IrisDeathCoroutine());
    }
    IEnumerator IrisDeathCoroutine()
    {
        print("LLM (large lethal machine) ran out of tokens(health) :(");
        yield return new WaitForSeconds(deathTimeSeconds);
        EndBossRoom();
        Destroy(gameObject);
    }
}
