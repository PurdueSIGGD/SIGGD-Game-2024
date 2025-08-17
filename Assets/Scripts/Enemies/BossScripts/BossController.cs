using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossController : MonoBehaviour
{

    [Header("Spawning and Enemy parameters")]
    EnemySpawning enemySpawner;
    [SerializeField] bool spawningEnabled = false;
    [SerializeField] int lowEnemyThreshold; // inclusive
    [SerializeField] float lowEnemyWaveSpawnSec;
    [SerializeField] float lowEnemyWaveSpawnTimer;
    [SerializeField] DamageContext killAllEnemiesContext;
    [SerializeField] int waveCounter = 0;

    [Header("Boss Identity Parameters")]
    GameObject bossObject;
    Health bossHealth;
    EnemyStateManager bossStateManager; // might be null

    [Header("Boss State Parameters")]
    [SerializeField] bool invincible = false;
    [SerializeField] bool defeated = false;

    void OnDestroy()
    {
        DisableInvincibility();
    }
    public void Start()
    {
        lowEnemyWaveSpawnTimer = lowEnemyWaveSpawnSec;
        enemySpawner = GameObject.Find("PersistentData").GetComponent<EnemySpawning>();
        bossHealth = GetComponent<Health>();
        bossStateManager = GetComponent<EnemyStateManager>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (spawningEnabled)
        {
            int numEnemies = GetNumEnemies();
            print("NUM ENEMIES: " + numEnemies);
            print("ENEMY: " + EnemySpawning.enemies[0]);

            // spawn wave if all enemies dead, or enemies left alive for too long
            if ((numEnemies <= 0) || (lowEnemyWaveSpawnTimer <= 0))
            {
                lowEnemyWaveSpawnTimer = lowEnemyWaveSpawnSec;
                SpawnWave();
            }
            if (numEnemies <= lowEnemyThreshold)
                lowEnemyWaveSpawnTimer -= Time.deltaTime;
        }
    }
    public void EnableInvincibility()
    {
        if (invincible) return;
        GameplayEventHolder.OnDamageFilter.Add(BossInvincibleDamageFilter);
        invincible = true;
    }
    public void DisableInvincibility()
    {
        if (!invincible) return;
        GameplayEventHolder.OnDamageFilter.Remove(BossInvincibleDamageFilter);
        invincible = false;
    }
    public void BossInvincibleDamageFilter(ref DamageContext context)
    {
        if (context.victim == bossHealth.gameObject)
        {
            context.damage = 0;
        }
    }
    public virtual void StartDefeatSequence()
    {
        defeated = true;
        StopSpawning();
        KillAllEnemies();
    }
    public void StartSpawning()
    {
        spawningEnabled = true;
        lowEnemyWaveSpawnTimer = lowEnemyWaveSpawnSec;
    }
    public void StopSpawning()
    {
        spawningEnabled = false;
    }
    public void SpawnEnemy(Vector2 position)
    {
        enemySpawner.SpawnEnemy(position);
    }
    public void SpawnWave()
    {
        enemySpawner.SpawnEnemyWave();
        waveCounter++;
    }
    public void KillAllEnemies()
    {
        killAllEnemiesContext.attacker = gameObject;
        killAllEnemiesContext.damage = 9999999999;
        enemySpawner.KillAllEnemies(killAllEnemiesContext);
    }
    public int GetNumEnemies()
    {
        return enemySpawner.GetCurrentEnemies().Count();
    }
    public int GetNumWaves()
    {
        return waveCounter;
    }
    public void EndBossRoom()
    {
        enemySpawner.EndRoom();
    }
}
