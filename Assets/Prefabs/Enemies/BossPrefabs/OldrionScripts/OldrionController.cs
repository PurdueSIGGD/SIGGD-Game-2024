using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OldrionController : BossController
{
    OldrionManager enemyStateManager;
    Animator anim;
    ActionPool actionPool;
    [SerializeField] float deathTimer;

    [Header("Fight State Params")]
    [SerializeField] int currentPhase; // 1, 2, or 3
    const int FINAL_PHASE = 3;

    [SerializeField] string lightActionName;
    [SerializeField] List<float> lightCooldowns = new List<float>();
    [SerializeField] string heavyActionName;
    [SerializeField] List<float> heavyCooldowns = new List<float>();
    [SerializeField] string dashActionName;
    [SerializeField] List<float> dashCooldowns = new List<float>();

    [SerializeField] HealingContext fullHealContext;

    public void Start()
    {
        base.Start();
        enemyStateManager = GetComponent<OldrionManager>();
        anim = GetComponent<Animator>();
        actionPool = GetComponent<ActionPool>();

        currentPhase = 1;
        UpdatePhase();
    }
    public void SpawnYokai(GameObject yokaiPrefab, GameObject enemy)
    {
        SpawnEnemyAtRandomPoint(enemy, yokaiPrefab);
    }

    public override void DefeatSequence()
    {
        currentPhase++;
        if (ShouldBossBeDefeated())
        {
            base.DefeatSequence();
            EnableInvincibility();
            enemyStateManager.DisableAllVFX();
            enemyStateManager.enabled = false;
            anim.SetTrigger("dead");
            StartCoroutine(DefeatCoroutine());
        }
        else
        {
            StartPrismaticSpiritCrush();
            SetPhase(currentPhase);
        }

    }
    /// <summary>
    ///  DO WHATEVER U WANT FOR THE END SEQUENCE IDC
    /// </summary>
    /// <returns></returns>
    IEnumerator DefeatCoroutine()
    {
        print("Bro no way I beat myself ts pmo sm ong :skull :skull :skull");
        yield return new WaitForSeconds(deathTimer);
        EndBossRoom();
    }
    bool ShouldBossBeDefeated()
    {
        return currentPhase > FINAL_PHASE;
    }
    public void UpdatePhase()
    {
        SetPhase(currentPhase);
    }
    void SetPhase(int phase)
    {
        int phaseIndex = phase - 1;
        actionPool.GetActionByName(lightActionName).SetCoolDown(lightCooldowns[phaseIndex]);
        actionPool.GetActionByName(heavyActionName).SetCoolDown(heavyCooldowns[phaseIndex]);
        actionPool.GetActionByName(dashActionName).SetCoolDown(dashCooldowns[phaseIndex]);
    }
    void StartPrismaticSpiritCrush()
    {
        print("AHHHHHH HEKPE MEEEE HELEPA!!!!!");
        fullHealContext.healing = bossHealth.GetStats().ComputeValue("Max Health");
        bossHealth.Heal(fullHealContext, this.gameObject);
    }
}
