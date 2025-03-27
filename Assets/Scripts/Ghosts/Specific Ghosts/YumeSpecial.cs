using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class YumeSpecial : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float chainRange = float.MaxValue;
    [SerializeField] private float flightSpeed;

    private Queue<GameObject> enemies;

    private ChainedEnemy head; // will usually be the first enemy hit by Yume's projectile
    private ChainedEnemy tail; // should always point to the end of the list
    private ChainedEnemy ptr;

    class ChainedEnemy
    {
        public GameObject enemy;
        public ChainedEnemy chainedTo;

        public ChainedEnemy() { enemy = null; chainedTo = null; }
    }

    void Start()
    {
        ptr = head = tail = new ChainedEnemy();
        enemies = new Queue<GameObject>();
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.V))
        {
            FateBind();
        }
    }

    public void FateBind()
    {
        // whenever ability fires, grab a copy of all enemies at play in a queue
        for (int i = 0; i < EnemySetTest.enemies.Count; i++)
        {
            GameObject enemy = EnemySetTest.enemies.Dequeue();
            enemies.Enqueue(enemy);
            EnemySetTest.enemies.Enqueue(enemy);
        }
        // now this.enemies should be populated with every enemy at play

        StartCoroutine(FireProjectile(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }


    // A recursive call used to construct a list of enemies chained to one another
    // by checking all enemies present and binding a hit enemy to the closest one
    private ChainedEnemy ChainNextEnemy(ChainedEnemy cur)
    {
        ChainedEnemy n = new ChainedEnemy();
        float minDist = chainRange;
        for (int i = 0; i < EnemySetTest.enemies.Count; i++)
        {
            GameObject enemy = EnemySetTest.enemies.Dequeue();

            if (enemy.GetInstanceID() == cur.enemy.GetInstanceID()) // if checking the currently linked enemy, pass
            {
                i--;
                continue; // do not add the removed enemy back to the list, the enemy is already linked
            }

            float dist = Vector2.Distance(cur.enemy.transform.position, enemy.transform.position); // cur must have an enemy

            if (dist < minDist)
            {
                minDist = dist;
                n.enemy = enemy;
            }

            EnemySetTest.enemies.Enqueue(enemy);
        }

        if (n.enemy == null) // and other condtions
        {
            return null; // end of the chain
        }

        // TODO shoot a projectile here to [n]
        // yield thread until function results, if projectile hits, continue
        // if projectile misses, return

        n.chainedTo = ChainNextEnemy(n); // else continue the chain

        return n;
    }

    // should find the next closest enemy to the one given
    private Transform FindNextTarget(GameObject cur)
    {
        Transform targetLoc = null;
        float minDist = chainRange;
        for (int i = 0; i < enemies.Count; i++)
        {
            GameObject enemy = enemies.Dequeue();

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

            enemies.Enqueue(enemy);
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
            tail.enemy = hitTarget;
            tail.chainedTo = new ChainedEnemy();
            tail = tail.chainedTo;

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
}
