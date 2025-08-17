using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossController : MonoBehaviour
{

    [Header("Spawning and Enemy parameters")]
    EnemySpawning enemySpawner;
    [SerializeField] bool waveSpawningEnabled = false;
    [SerializeField] int lowEnemyThreshold; // inclusive
    [SerializeField] float lowEnemyWaveSpawnSec;
    float lowEnemyWaveSpawnTimer;
    [SerializeField] bool passiveSpawningEnabled = false;
    [SerializeField] float passiveSpawnTimeSec;
    float passiveSpawnTimer;
    [SerializeField] DamageContext killAllEnemiesContext;
    [SerializeField] int waveCounter = 0;
    [SerializeField] int enemiesKilledCounter = 0;

    Health bossHealth;
    EnemyStateManager bossStateManager; // might be null

    [Header("Boss State Parameters")]
    [SerializeField] bool invincible = false;
    [SerializeField] bool defeated = false;

    void Awake()
    {
        GameplayEventHolder.OnDeath += CheckEnemyDeathOnDeath;
    }
    void OnDestroy()
    {
        GameplayEventHolder.OnDeath -= CheckEnemyDeathOnDeath;
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
        if (waveSpawningEnabled)
        {
            int numEnemies = GetNumEnemies();

            // spawn wave if all enemies dead, or enemies left alive for too long
            if ((numEnemies <= 0) || (lowEnemyWaveSpawnTimer <= 0))
            {
                lowEnemyWaveSpawnTimer = lowEnemyWaveSpawnSec;
                SpawnWave();
            }
            if (numEnemies <= lowEnemyThreshold)
                lowEnemyWaveSpawnTimer -= Time.deltaTime;
        }

        if (passiveSpawningEnabled)
        {
            passiveSpawnTimer -= Time.deltaTime;
            if (passiveSpawnTimer <= 0)
            {
                SpawnEnemyAtRandomPoint();
                passiveSpawnTimer = passiveSpawnTimeSec;
            }
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
        StopWaveSpawning();
        KillAllEnemies();
    }
    public void StartWaveSpawning()
    {
        waveSpawningEnabled = true;
        lowEnemyWaveSpawnTimer = lowEnemyWaveSpawnSec;
    }
    public void StopWaveSpawning()
    {
        waveSpawningEnabled = false;
    }
    public void StartPassiveSpawning()
    {
        passiveSpawningEnabled = true;
        passiveSpawnTimer = passiveSpawnTimeSec;
    }
    public void StopPassiveSpawning()
    {
        passiveSpawningEnabled = false;
    }
    public void SpawnEnemyAtRandomPoint()
    {
        enemySpawner.SpawnEnemyAtRandomPoint();
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
    public void CheckEnemyDeathOnDeath(DamageContext context)
    {
        if (context.victim.CompareTag("Enemy"))
        {
            enemiesKilledCounter++;
        }
    }
    public int GetNumEnemiesKilled()
    {
        return enemiesKilledCounter;
    }
    public void EndBossRoom()
    {
        enemySpawner.EndRoom();
    }
}
