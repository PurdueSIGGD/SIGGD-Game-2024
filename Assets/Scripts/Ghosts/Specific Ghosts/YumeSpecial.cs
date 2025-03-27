using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class YumeSpecial : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float chainRange = float.MaxValue;

    private Queue<GameObject> enemies; // 

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
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.V))
        {
            ChainAllEnemies();
        }
    }

    // TODO rename, add comments
    public void ChainAllEnemies()
    {
        // whenever ability fires, grab a copy of all enemies at play in a queue
        for (int i = 0; i < EnemySetTest.enemies.Count; i++)
        {
            GameObject enemy = EnemySetTest.enemies.Dequeue();
            enemies.Enqueue(enemy);
            EnemySetTest.enemies.Enqueue(enemy);
        }
        // now this.enemies should be populated with every enemy at play

        // TODO fire projectile at mouse direction
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

    private IEnumerator FireProjectile(Vector2 dest)
    {
        YumeProjectile yumeProjectile = Instantiate(projectile, transform).GetComponent<YumeProjectile>();

        yield return new WaitUntil(yumeProjectile.hasExpired);

        // if the projectile has hit an enemy
        GameObject hitTarget = yumeProjectile.getHitTarget();
        if (hitTarget != null)
        {
            // then add the hit enemy to linked list
            tail.enemy = hitTarget;
            tail = tail.chainedTo;

            // find next target position and fire
            Vector2 targetPos = FindNextTarget(hitTarget).position;
            StartCoroutine(FireProjectile(targetPos));
        }
        
    }
}
