using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoboruController : BossController
{
    EnemyStateManager enemyStateManager;
    Animator anim;
    [SerializeField] float deathTimer;
    [SerializeField] NoboruManager manager;

    public void Start()
    {
        base.Start();
        enemyStateManager = GetComponent<EnemyStateManager>();
        anim = GetComponent<Animator>();
    }

    public override void EnableAI()
    {
        base.EnableAI();
        manager.enabled = true;
        AudioManager.Instance.GetComponentInChildren<MusicManager>().CrossfadeTo(MusicTrackName.NOBORU_THEME, 0.5f);
    }
    public void SpawnYokai(GameObject yokaiPrefab, GameObject enemy)
    {
        SpawnEnemyAtRandomPoint(enemy);
    }

    public override void DefeatSequence()
    {
        base.DefeatSequence();
        EnableInvincibility();
        enemyStateManager.enabled = false;
        anim.SetTrigger("dead");
        StartCoroutine(DefeatCoroutine());
        AchievementTracker.instance.SetAchievement(AchievementTracker.instance.noboruName);
    }
    IEnumerator DefeatCoroutine()
    {
        print("Maybe... maybe I shouldn't have been the bad guy... maybe I shouldn't have killed *ALL* of them...");
        yield return new WaitForSeconds(deathTimer);
        //Destroy(gameObject);
        EndBossRoom();
    }
}
