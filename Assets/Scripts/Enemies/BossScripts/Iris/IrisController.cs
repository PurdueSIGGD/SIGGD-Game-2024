using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrisController : BossController
{
    [Header("IRIS VARIABLES")]
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

    [Header("Railshot/Laser parameters")]
    [SerializeField] IrisLaser irisLaser;
    [SerializeField] float laserFireIntervalSec; // includes laser fire time, so shouldn't be shorter than total laser fire sequence time 
    float laserTimer;

    [Header("Death Implementation parameters")]
    [SerializeField] float deathTimeSeconds;



    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += OnPlayerDamageTaken;
        GameplayEventHolder.OnDeath += OnDeath;
        GameplayEventHolder.OnDamageDealt += OnDamageTaken;
        GameplayEventHolder.OnDeath += OnPlayerDeath;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= OnPlayerDamageTaken;
        GameplayEventHolder.OnDeath -= OnDeath;
        GameplayEventHolder.OnDamageDealt -= OnDamageTaken;
        GameplayEventHolder.OnDeath -= OnPlayerDeath;
    }

    public new void Start()
    {
        base.Start();
        ActivateShield(false);
        if (irisLaser != null)
        {
            irisLaser.Initialize(this.gameObject);
        }
    }
    public new void Update()
    {
        base.Update();

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
        visualManager.SetVisualState((shieldOn) ? (damageState) : (IrisVisualStates.DAMAGE_HIGH));

        // laser firing loop
        if (!IsDefeated() && bossActivated)
        {
            laserTimer -= Time.deltaTime;
            if (laserTimer < 0 && PlayerID.instance != null)
            {
                irisLaser.FireSequence(PlayerID.instance.gameObject);
                laserTimer = laserFireIntervalSec;
            }
        }
    }

    public override void EnableAI()
    {
        base.EnableAI();
        AudioManager.Instance.GetComponentInChildren<MusicManager>().CrossfadeTo(MusicTrackName.IRIS_THEME, 0.5f);
    }
    public void ActivateShield(bool playSFX)
    {
        if (playSFX)
        {
            AudioManager.Instance.SFXBranch.PlaySFXTrack("IRISShieldUp");
            AudioManager.Instance.SFXBranch.StopSFXTrack("IRISShieldDownLoop");
            AudioManager.Instance.VABranch.PlayVATrack("IRIS Shield Up");
        }
        ToggleShield(true);
    }
    public void DeactivateShield()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("IRISShieldDown");
        AudioManager.Instance.SFXBranch.PlaySFXTrack("IRISShieldDownLoop");
        AudioManager.Instance.VABranch.PlayVATrack("IRIS Shield Down");
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
            ActivateShield(true);
    }
    public override void DefeatSequence()
    {
        base.DefeatSequence();
        AudioManager.Instance.SFXBranch.StopSFXTrack("IRISShieldDownLoop");
        visualManager.ActivateDeathVisual();
        irisLaser.Stop();
        StartCoroutine(IrisDeathCoroutine());
    }
    IEnumerator IrisDeathCoroutine()
    {
        print("LLM (large lethal machine) ran out of tokens(health) :(");
        yield return new WaitForSeconds(deathTimeSeconds);
        EndBossRoom();
        Destroy(gameObject);
    }



    public void OnDamageTaken(DamageContext context)
    {
        if (context.victim != gameObject) return;

        if (gameObject.GetComponent<Health>().currentHealth <= 400f)
        {
            AudioManager.Instance.VABranch.PlayVATrack("IRIS Significant Damage Taken");
            return;
        }
        AudioManager.Instance.VABranch.PlayVATrack("IRIS Light Damage Taken");
    }

    public void OnDeath(DamageContext context)
    {
        if (context.victim != gameObject) return;

        AudioManager.Instance.VABranch.PlayVATrack("IRIS On Boss Death");
    }

    public void OnPlayerDamageTaken(DamageContext context)
    {
        if (!context.victim.CompareTag("Player")) return;
        if (!context.attacker.Equals(gameObject)) return;

        AudioManager.Instance.VABranch.PlayVATrack("IRIS Damaging Player");
    }

    public void OnPlayerDeath(DamageContext context)
    {
        if (!context.victim.CompareTag("Player")) return;

        AudioManager.Instance.VABranch.PlayVATrack("IRIS Player Death");
    }
}
