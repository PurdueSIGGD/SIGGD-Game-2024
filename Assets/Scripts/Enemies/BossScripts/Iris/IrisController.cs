using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrisController : BossController
{


    [Header("IRIS VARIABLES")]
    [SerializeField] bool startSpawn;

    Animator animator;
    int damageState = 0; // 0:none, 1:low, 2:high
    List<int> damageStateThresholds = new List<int>();

    [Header("Shield parameters")]
    [SerializeField] GameObject shieldVisual;
    [SerializeField] GameObject shieldUIVisual;
    [SerializeField] bool shieldOn;
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
        animator = GetComponent<Animator>();
        ActivateShield();
    }
    public new void Update()
    {
        base.Update();
        if (startSpawn)
        {
            StartSpawning();
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
        shieldVisual.SetActive(val);
        shieldUIVisual.SetActive(val);
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
        ActivateShield();
        wavesSinceShieldUp = 0;
        enemiesSinceShieldUp = 0;
    }
    public override void StartDefeatSequence()
    {
        base.StartDefeatSequence();
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
