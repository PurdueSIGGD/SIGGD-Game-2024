using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoboruController : BossController
{
    EnemyStateManager enemyStateManager;
    Animator anim;
    [SerializeField] float deathTimer;

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
    IEnumerator DefeatCoroutine()
    {
        print("Maybe... maybe I shouldn't have been the bad guy... maybe I shouldn't have killed *ALL* of them...");
        yield return new WaitForSeconds(deathTimer);
        Destroy(gameObject);
        EndBossRoom();
    }
}
