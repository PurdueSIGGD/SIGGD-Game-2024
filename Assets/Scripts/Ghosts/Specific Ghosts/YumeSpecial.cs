using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class YumeSpecial : MonoBehaviour
{
    [SerializeField] private float chainRange = float.MaxValue;


    private ChainedEnemy head; // will usually be the first enemy hit by Yume's projectile
    private ChainedEnemy ptr;
    
    class ChainedEnemy
    {
        public GameObject enemy;
        public ChainedEnemy chainedTo;

        public ChainedEnemy() { enemy = null; chainedTo = null; }
    }

    void Start()
    {
        ptr = head = new ChainedEnemy();
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.V))
        {
            ChainAllEnemies();
        }
    }

    public void ChainAllEnemies()
    {
        ptr.enemy = EnemySetTest.enemies.Dequeue();
        ptr.chainedTo = ChainNextEnemy(ptr);
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
}
