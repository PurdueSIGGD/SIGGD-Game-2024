using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class YumeSpecial : MonoBehaviour
{
    public SeamstressManager manager;
    PlayerStateMachine psm;
    EnemySpawning enemySpawning;

    class ChainedEnemy
    {
        public GameObject enemy;
        public ChainedEnemy chainedTo;

        public ChainedEnemy() { enemy = null; chainedTo = null; }
    }

    void Start()
    {
        psm = GetComponent<PlayerStateMachine>();
        enemySpawning = PersistentData.Instance.GetComponent<EnemySpawning>();
    }

    void Update()
    {
        if (manager != null)
        {
            if (manager.getSpecialCooldown() > 0)
            {
                psm.OnCooldown("c_special");
            }
            else
            {
                psm.OffCooldown("c_special");
            }
        }
    }

    public void StartDash()
    {
        // whenever ability fires, grab a copy of all enemies at play in a queue
        foreach (GameObject enemy in enemySpawning.GetCurrentEnemies())
        {
            manager.linkableEnemies.Enqueue(enemy);
        }
        // now this.enemies should be populated with every enemy at play
        manager.ResetDuration();
        manager.startSpecialCooldown();
        StartCoroutine(FireProjectile(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }

    private IEnumerator FireProjectile(Vector2 orig, Vector2 dest)
    {
        orig = orig + (dest - orig).normalized * 2;
        YumeProjectile yumeProjectile = Instantiate(manager.projectile, orig, transform.rotation).GetComponent<YumeProjectile>();
        yumeProjectile.Initialize(dest, manager.flightSpeed, manager.chainRange, manager);

        yield return new WaitUntil(yumeProjectile.HasExpired); // wait until the projectile has hit or is destroyed

        // if the projectile has hit an enemy
        GameObject hitTarget = yumeProjectile.GetHitTarget();
        if (hitTarget != null)
        {
            // then add the hit enemy to linked list
            FateboundDebuff debuff =  hitTarget.AddComponent<FateboundDebuff>();
            debuff.manager = manager;

            manager.AddEnemy(hitTarget);

            // find next target position and fire
            Transform targetPos = manager.FindNextTarget(hitTarget);
            if (targetPos == null || manager.IncrementRicochet())
            {
                manager.ResetRicochet();
                yield return null;
            }
            else
            {
                StartCoroutine(FireProjectile(hitTarget.transform.position, targetPos.position));
            }
        }
    }
}
