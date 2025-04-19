using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// Placeholder class for testing yume speical ability
/// Expect to be replaced after enemy spawn is implemented
/// </summary>
[DisallowMultipleComponent]
public class EnemySetTest : MonoBehaviour
{
    public static EnemySetTest Instance;
    public static Queue<GameObject> enemies = new Queue<GameObject>();
    void Start()
    {
        Instance = this;
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Enqueue(gameObject);
        }
        GameplayEventHolder.OnDeath += RemoveDeadEnemies;
    }

    /// <summary>
    /// Removes an enemy from the queue by its instance ID, useful for when an enemy dies
    /// </summary>
    /// <param name="enemyID"></param>
    public void RemoveEnemy(int enemyID)
    {
        int i = 0;
        while (i < enemies.Count)
        {
            i++;
            if (enemies.Dequeue().GetInstanceID() == enemyID)
            {
                return;
            }
            enemies.Enqueue(gameObject);
        }
    }

    private void RemoveDeadEnemies(DamageContext context)
    {
        if (context.victim.CompareTag("Enemy"))
        {
            RemoveEnemy(context.victim.GetInstanceID());
        }
    }
}
