using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class YumeSpecial : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float chainRange = float.MaxValue;
    [SerializeField] private float flightSpeed;
    [SerializeField] private float chainedDuration;

    [Header("Fatebound Effect")]
    [SerializeField] private float sharedDmgScaling;

    private Queue<GameObject> linkableEnemies;
    private ChainedEnemy head; // will usually be the first enemy hit by Yume's projectile
    private ChainedEnemy tail; // should always point to the end of the list
    private ChainedEnemy ptr;

    private float durationCounter;

    class ChainedEnemy
    {
        public GameObject enemy;
        public ChainedEnemy chainedTo;

        public ChainedEnemy() { enemy = null; chainedTo = null; }
    }

    void Start()
    {
        ptr = head = tail = new ChainedEnemy();
        linkableEnemies = new Queue<GameObject>();
        durationCounter = chainedDuration;
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.V))
        {
            FateBind();
        }
        if (head != null) // if linked list isn't empty
        {
            durationCounter -= Time.deltaTime;
            if (durationCounter < 0)
            {
                ClearList();
            }
        }
    }

    public void FateBind()
    {
        // whenever ability fires, grab a copy of all enemies at play in a queue
        for (int i = 0; i < EnemySetTest.enemies.Count; i++)
        {
            GameObject enemy = EnemySetTest.enemies.Dequeue();
            linkableEnemies.Enqueue(enemy);
            EnemySetTest.enemies.Enqueue(enemy);
        }
        // now this.enemies should be populated with every enemy at play
        durationCounter = chainedDuration;
        StartCoroutine(FireProjectile(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }

    public void DamageLinkedEnemies(int enemyID, DamageContext context)
    {
        ptr = head;

        while (ptr.enemy != null)
        {
            if (ptr.enemy.GetInstanceID() != enemyID)
            {
                DamageContext sharedDmg = new DamageContext();
                sharedDmg.damage = context.damage * sharedDmgScaling;
                sharedDmg.damageStrength = context.damageStrength;

                ptr.enemy.GetComponent<Health>().NoContextDamage(sharedDmg, PlayerID.instance.gameObject);
            }
            ptr = ptr.chainedTo;
        }
    }

    // should find the next closest enemy to the one given
    private Transform FindNextTarget(GameObject cur)
    {
        Transform targetLoc = null;
        float minDist = chainRange;
        for (int i = 0; i < linkableEnemies.Count; i++)
        {
            GameObject enemy = linkableEnemies.Dequeue();

            if (enemy.GetInstanceID() == cur.GetInstanceID()) // if checking the currently linked enemy, pass
            {
                i--;
                continue; // do not add the removed enemy back to the list, the enemy is already linked
            }

            float dist = Vector2.Distance(enemy.transform.position, cur.transform.position);

            if (dist < minDist)
            {
                targetLoc = enemy.transform;
                minDist = dist;
            }

            linkableEnemies.Enqueue(enemy);
        }
        return targetLoc;
    }

    private IEnumerator FireProjectile(Vector2 orig, Vector2 dest)
    {
        orig = orig + (dest - orig).normalized * 2;
        YumeProjectile yumeProjectile = Instantiate(projectile, orig, transform.rotation).GetComponent<YumeProjectile>();
        yumeProjectile.Initialize(dest, flightSpeed, chainRange);

        yield return new WaitUntil(yumeProjectile.HasExpired);

        // if the projectile has hit an enemy
        GameObject hitTarget = yumeProjectile.GetHitTarget();
        if (hitTarget != null)
        {
            // then add the hit enemy to linked list
            hitTarget.AddComponent<FateboundDebuff>();

            if (head.enemy == null)
            {
                head.enemy = hitTarget;
                tail = head.chainedTo = new ChainedEnemy();
            }
            else
            {
                tail.enemy = hitTarget;
                tail.chainedTo = new ChainedEnemy();
                tail = tail.chainedTo;
            }

            // find next target position and fire
            Transform targetPos = FindNextTarget(hitTarget);
            if (targetPos == null)
            {
                yield return null;
            }
            else
            {
                StartCoroutine(FireProjectile(hitTarget.transform.position, targetPos.position));
            }
        }
    }

    private void ClearList()
    {
        ptr = head = tail = new ChainedEnemy();
    }
}
