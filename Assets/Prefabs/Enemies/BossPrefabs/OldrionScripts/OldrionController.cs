using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldrionController : BossController
{
    EnemyStateManager enemyStateManager;
    Animator anim;
    [SerializeField] float deathTimer;

    [Header("Fight State Params")]

    int currentPhase; // 1, 2, or 3

    public void Start()
    {
        base.Start();
        enemyStateManager = GetComponent<EnemyStateManager>();
        anim = GetComponent<Animator>();
    }
    public void SpawnYokai(GameObject yokaiPrefab, GameObject enemy)
    {
        SpawnEnemyAtRandomPoint(enemy, yokaiPrefab);
    }

    public override void DefeatSequence()
    {
        base.DefeatSequence();
        EnableInvincibility();
        enemyStateManager.enabled = false;
        anim.SetTrigger("dead");
        StartCoroutine(DefeatCoroutine());
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
}
