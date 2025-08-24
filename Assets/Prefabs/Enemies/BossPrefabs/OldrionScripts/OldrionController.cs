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
    bool crushing;

    public void Start()
    {
        base.Start();
        enemyStateManager = GetComponent<OldrionManager>();
        anim = GetComponent<Animator>();
        actionPool = GetComponent<ActionPool>();

        currentPhase = 1;
        UpdateCooldowns();
    }

    public override void DefeatSequence()
    {
        if (crushing)
        {
            return;
        }

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
    public void UpdateCooldowns()
    {
        SetCooldownsByPhase(currentPhase);
    }
    void SetCooldownsByPhase(int phase)
    {
        int phaseIndex = phase - 1;
        actionPool.GetActionByName(lightActionName).SetCoolDown(lightCooldowns[phaseIndex]);
        actionPool.GetActionByName(heavyActionName).SetCoolDown(heavyCooldowns[phaseIndex]);
        actionPool.GetActionByName(dashActionName).SetCoolDown(dashCooldowns[phaseIndex]);
    }
    void StartPrismaticSpiritCrush()
    {
        SetCrushing(true);
        EnableInvincibility();
        enemyStateManager.DisableAllVFX();
        StartCoroutine(SpiritCrushCoroutine());
    }
    IEnumerator SpiritCrushCoroutine()
    {
        anim.ResetTrigger("precrush");
        anim.SetTrigger("precrush");

        anim.ResetTrigger("crush");
        // start of spirit crush

        print("UNC: heh... you're pretty strong...");
        yield return new WaitForSeconds(1f);

        print("UNC: yet... after all your trials... all your efforts");
        yield return new WaitForSeconds(1f);

        print("UNC: it was  ALL  FOR  NAUGHT!!!");
        print("UNC: RAHHHHHH");

        anim.SetTrigger("crush");
    }
    void OnCrush()
    {
        fullHealContext.healing = bossHealth.GetStats().ComputeValue("Max Health");
        bossHealth.Heal(fullHealContext, this.gameObject);
    }
    void OnCrushEnd()
    {
        DisableInvincibility();
        UpdateCooldowns();
        SetCrushing(false);
    }
    void SetCrushing(bool val)
    {
        crushing = val;
        enemyStateManager.SetEnemyManagerCrushing(val);
    }
}
