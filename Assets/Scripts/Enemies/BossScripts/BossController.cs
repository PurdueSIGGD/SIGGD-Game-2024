using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossController : MonoBehaviour
{

    [Header("Spawning and Enemy parameters")]
    [SerializeField] protected bool startSpawn;
    EnemySpawning enemySpawner;
    bool waveSpawningEnabled = false;
    [SerializeField] int lowEnemyThreshold; // inclusive
    [SerializeField] float lowEnemyWaveSpawnSec;
    float lowEnemyWaveSpawnTimer;
    bool passiveSpawningEnabled = false;
    [SerializeField] float passiveSpawnTimeSec;
    float passiveSpawnTimer;
    [SerializeField] DamageContext killAllEnemiesContext;
    int waveCounter = 0;
    int enemiesKilledCounter = 0;
    bool triggerNewWave = false;

    protected Health bossHealth;
    protected EnemyStateManager bossStateManager; // might be null

    [Header("Boss State Parameters")]
    bool invincible = false;
    bool defeated = false;

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
    public void Update()
    {
        if (waveSpawningEnabled)
        {
            int numEnemies = GetNumEnemies();

            // spawn wave if all enemies dead, or enemies left alive for too long
            if (triggerNewWave || (lowEnemyWaveSpawnTimer <= 0))
            {
                lowEnemyWaveSpawnTimer = lowEnemyWaveSpawnSec;
                triggerNewWave = false;
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

    public void EnableAI()
    {
        startSpawn = true;
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
        StopPassiveSpawning();
        KillAllEnemies();
    }
    public void StartWaveSpawning()
    {
        waveSpawningEnabled = true;
        lowEnemyWaveSpawnTimer = lowEnemyWaveSpawnSec;
        SpawnWave();
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
    public void SpawnEnemyAtRandomPoint(GameObject enemy = null, GameObject orb = null)
    {
        enemySpawner.SpawnEnemyAtRandomPoint(enemy, orb);
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
            if ((enemySpawner.GetCurrentEnemies().Contains(context.victim) && GetNumEnemies() == 1) ||
                 (!enemySpawner.GetCurrentEnemies().Contains(context.victim) && GetNumEnemies() == 0))
            {
                triggerNewWave = true;
            }
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
    public bool IsDefeated()
    {
        return defeated;
    }
}
