using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class YumeSpecial : MonoBehaviour
{
    // TODO remove
    private Queue<GameObject> enemies;
    private Queue<GameObject> hitEnemies = new Queue<GameObject>();
    private node ptr;
    
    // TODO remove
    class node
    {
        public GameObject enemy;
        public node next;

        public node(GameObject enemy, node next)
        {
            this.enemy = enemy;
            this.next = next;
        }

        public node(GameObject enemy)
        {
            this.enemy = enemy;
            this.next = null;
        }

        public node() 
        {
            enemy = null;
            next = null;
        }
    }

    void Start()
    {
        ptr = new node();
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.V))
        {
            ChainAllEnemies();
            while (ptr != null)
            {
                Debug.Log(ptr.enemy.name);
                ptr = ptr.next;
            }
        }
    }

    public void ChainAllEnemies()
    {
        ptr.enemy = EnemySetTest.enemies.Dequeue();
        ptr.next = ChainNextEnemy(ptr);
    }

    private node ChainNextEnemy(node cur)
    {
        node n = new node();
        float minDist = float.MaxValue;
        for (int i = 0; i < EnemySetTest.enemies.Count; i++)
        {
            GameObject enemy = EnemySetTest.enemies.Dequeue();

            if (enemy.GetInstanceID() == cur.enemy.GetInstanceID()) // if checking the currently linked enemy, pass
            {
                i--;
                continue;
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

        n.next = ChainNextEnemy(n); // else continue the chain

        return n;
    }
}
