using System.Collections;
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
        if (manager.GetSpools() >= manager.GetStats().ComputeValue("Special Attack Spools Needed"))
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
            manager.AddSpools((int)-manager.GetStats().ComputeValue("Special Attack Spools Needed"));

            // fire any events waiting for special ability to activate
            GameplayEventHolder.OnAbilityUsed.Invoke(manager.specialContext);
        }
    }

    private IEnumerator FireProjectile(Vector2 orig, Vector2 dest)
    {
        YumeProjectile yumeProjectile = Instantiate(manager.projectile, orig, transform.rotation).GetComponent<YumeProjectile>();
        yumeProjectile.Initialize(dest, manager.GetStats().ComputeValue("Projectile Flight Speed"), manager.GetStats().ComputeValue("Projectile Enemy Chain Range"), manager);

        yield return new WaitUntil(yumeProjectile.HasExpired); // wait until the projectile has hit or is destroyed

        // if the projectile has hit an enemy
        GameObject hitTarget = yumeProjectile.GetHitTarget();
        if (hitTarget != null)
        {
            // then add the hit enemy to linked list
            manager.AddEnemy(hitTarget);
            FateboundDebuff debuff =  hitTarget.AddComponent<FateboundDebuff>();
            debuff.manager = manager;

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
