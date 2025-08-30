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
    [SerializeField] string comboActionName;
    [SerializeField] List<float> comboCooldowns = new List<float>();

    [Header("Spirit Crushing Shenanigans")]
    DropManager dropManager;
    [SerializeField] HealingContext fullHealContext;
    [SerializeField] DamageContext aoeDamage;
    [SerializeField] GameObject aoeWarning;
    [SerializeField] GameObject aoeVisual;
    [SerializeField] Transform aoeTrigger;
    [SerializeField] float aoeDamageVal;
    bool crushing;
    [SerializeField] DropTable spiritTable;
    [SerializeField] Transform spiritCrushedSpawn;


    public void Start()
    {
        base.Start();
        enemyStateManager = GetComponent<OldrionManager>();
        anim = GetComponent<Animator>();
        actionPool = GetComponent<ActionPool>();
        dropManager = GetComponent<DropManager>();

        currentPhase = 1;
        UpdateCooldowns();
        GETANGRY();
    }
    void Update()
    {
        bossHealth.isAlive = !IsDefeated();
        base.Update();
    }

    public void GETANGRY()
    {
        enemyStateManager.enabled = true;
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
            enemyStateManager.StopAllActions();
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

        // TRIGGER FINAL CUTSCENE STUFF HERE!!!
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
        actionPool.GetActionByName(comboActionName).SetCoolDown(comboCooldowns[phaseIndex]);
    }
    void StartPrismaticSpiritCrush()
    {
        SetCrushing(true);
        EnableInvincibility();
        enemyStateManager.StopAllActions();
        aoeWarning.SetActive(true);
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

        //

        anim.SetTrigger("crush");
    }
    void OnCrush()
    {
        aoeWarning.SetActive(false);
        aoeVisual.SetActive(true);
        aoeDamage.damage = aoeDamageVal;
        enemyStateManager.AoeDamage(aoeTrigger, aoeDamage);

        fullHealContext.healing = bossHealth.GetStats().ComputeValue("Max Health");
        bossHealth.Heal(fullHealContext, this.gameObject);

        DropSpirits(spiritTable, spiritCrushedSpawn.gameObject);
    }
    void OnCrushEnd()
    {
        aoeVisual.SetActive(false);
        DisableInvincibility();
        UpdateCooldowns();
        SetCrushing(false);
    }
    void SetCrushing(bool val)
    {
        crushing = val;
        enemyStateManager.SetEnemyManagerCrushing(val);
    }
    void DropSpirits(DropTable table, GameObject victim)
    {
        foreach (DropTable.Drop drop in table.dropTable)
        {
            // Decide if each loot will drop
            float r = UnityEngine.Random.value;
            if (r > drop.chance)
            {
                continue;
            }

            // Decdie how much to drop
            r = UnityEngine.Random.value;
            float dropCount = (int)((drop.maxCount - drop.minCount) * r + drop.minCount);

            float xDeviation = -0.0003f;
            float yDeviation = 0.00003f;

            for (int i = 0; i < dropCount; i++)
            {
                Rigidbody2D rb = Instantiate(drop.obj, victim.transform.position, victim.transform.rotation).GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(UnityEngine.Random.value * xDeviation, yDeviation), ForceMode2D.Impulse);
                xDeviation = -xDeviation;
            }
        }
    }
}
