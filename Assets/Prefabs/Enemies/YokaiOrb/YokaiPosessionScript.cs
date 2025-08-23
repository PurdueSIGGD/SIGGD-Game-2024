using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YokaiPosessionScript : MonoBehaviour
{

    EnemySpawning enemySpawnManager;
    GameObject parentEnemyInstance;
    [SerializeField] GameObject parentEnemyPrefab;
    [SerializeField] GameObject yokaiOrbPrefab;

    void Awake()
    {
        GameplayEventHolder.OnDeath += OnDeath;
    }
    void OnDestroy()
    {
        GameplayEventHolder.OnDeath -= OnDeath;
    }
    void Start()
    {
        enemySpawnManager = FindFirstObjectByType<EnemySpawning>();
        parentEnemyInstance = this.transform.parent.gameObject;
        print(parentEnemyPrefab.name);
    }
    public void OnDeath(DamageContext damageContext)
    {
        if (damageContext.victim == parentEnemyInstance)
        {
            print("POSESSION VICTIM: " + damageContext.victim);
            enemySpawnManager.SpawnEnemyWithDelay(this.transform.position, 0.1f, parentEnemyPrefab, yokaiOrbPrefab);
        }
    }
}
