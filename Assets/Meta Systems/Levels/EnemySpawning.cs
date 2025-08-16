using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    public static EnemySpawning instance;
    public static EnemySpawn[] enemies;
    public bool roomCleared; // toggled true when all enemies are killed and no more can be spawned
    [SerializeField] int startMinEnemiesSpawn;
    [SerializeField] int startMaxEnemiesSpawn;
    [SerializeField] int endMinEnemiesSpawn;
    [SerializeField] int endMaxEnemiesSpawn;
    [SerializeField] int startWaveNum;
    [SerializeField] int endWaveNum;

    [Header("Enemy directional indicator")]
    [SerializeField] int showEnemyThreshold;
    [SerializeField] GameObject enemyIndicator;
    [SerializeField] private GameObject doorIndicator;

    private List<GameObject> currentEnemies = new List<GameObject>();
    private int waveNumber;
    private int currentMaxWave;
    private GameObject[] spawnPoints;
    private bool showRemainingEnemy;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        instance = this;
        GameplayEventHolder.OnDeath += OnDeath;
    }


    /// <summary>
    /// Event to handle enemy deaths for the enemy spawn manager
    /// </summary>
    /// <param name="context"></param>
    public void OnDeath(DamageContext context)
    {
        if (currentEnemies.Contains(context.victim))
        {
            // remove the dead enemy from the list
            Debug.Log(context.victim.name + " has died");
            currentEnemies.Remove(context.victim);
            EnemiesLeftUpdater.enemiesLeft = currentEnemies.Count;

            // handle scenario when the last enemy left in the list dies
            // increment wave if not last wave, else end room and activate door
            if (currentEnemies.Count <= 0)
            {
                if (waveNumber + 1 < currentMaxWave)
                {
                    SpawnEnemyWave();
                    waveNumber += 1;
                }
                else
                {
                    Door.activateDoor(true);
                    roomCleared = true;
                    foreach (Door door in GameObject.FindObjectsOfType<Door>())
                    {
                        Instantiate(doorIndicator, door.transform.position, Quaternion.identity);
                    }
                }
            }
        }

        // hides 'enemies remaining' UI on player death
        if (context.victim.gameObject.CompareTag("Player"))
        {
            EnemiesLeftUpdater.enemiesLeft = -1;
        }

        // show visual indicators on screen
        ShowIndicators();
    }


    /// <summary>
    /// Returns a random enemy gameobject following the rules of the current enemy pool and spawn chances
    /// </summary>
    /// <returns></returns>
    private GameObject GetNextEnemy()
    {
        float totalChance = 0.0f;
        for (int i = 0; i < enemies.Length; i++)
        {
            totalChance += Mathf.Lerp(enemies[i].GetStartChance(), enemies[i].GetEndChance(), GetComponent<LevelSwitching>().GetProgress());
        }

        float rng = Random.Range(0.0f, totalChance);
        float counter = 0.0f;
        for (int j = 0; j < enemies.Length; j++)
        {
            counter += Mathf.Lerp(enemies[j].GetStartChance(), enemies[j].GetEndChance(), GetComponent<LevelSwitching>().GetProgress());
            if (rng < counter)
            {
                return enemies[j].GetEnemy();
            }
        }
        return null;
    }

    /// <summary>
    /// Spawns a single wave of enemies
    /// </summary>
    public void SpawnEnemyWave()
    {
        // randomize spawn location order
        ReshufflePoints(ref spawnPoints);

        // calculate number of enemies to spawn
        int numEnemies = Mathf.FloorToInt(
            Random.Range(
                Mathf.Lerp(startMinEnemiesSpawn, endMinEnemiesSpawn, GetComponent<LevelSwitching>().GetProgress()),
                Mathf.Lerp(startMaxEnemiesSpawn, endMaxEnemiesSpawn, GetComponent<LevelSwitching>().GetProgress())
            )
        );

        // spawn enemies
        for (int i = 0; i < Mathf.Min(numEnemies, spawnPoints.Length); i++)
        {
            GameObject newEnemy = Instantiate(GetNextEnemy());
            RegisterNewEnemy(newEnemy);
            // currentEnemies.Add(newEnemy);
            // newEnemy.transform.position = spawnPoints[i].transform.position;
        }
        // // update UI to reflect new enemies
        // EnemiesLeftUpdater.enemiesLeft = currentEnemies.Count;
        // LevelProgressUpdater.progress = GetComponent<LevelSwitching>().GetProgress();
        ShowIndicators();
    }

    private void ShowIndicators()
    {
        if (EnemiesLeftUpdater.enemiesLeft > 0 &&
            EnemiesLeftUpdater.enemiesLeft < showEnemyThreshold)
        {
            foreach (GameObject enemy in currentEnemies)
            {
                if (enemy.GetComponentInChildren<DirectionalArrowBehaviour>()) continue;
                Instantiate(enemyIndicator, enemy.transform);
            }
            showRemainingEnemy = true;
        }
        else if (showRemainingEnemy)
        {
            foreach (GameObject enemy in currentEnemies)
            {
                DirectionalArrowBehaviour excessIndicator = enemy.GetComponentInChildren<DirectionalArrowBehaviour>();
                if (excessIndicator && excessIndicator.gameObject != null) Destroy(excessIndicator.gameObject);
            }
            showRemainingEnemy = false;
        }
    }

    public void StartLevel()
    {
        FindObjectOfType<LevelMusicDriver>().enemySpawning = this;
        Door.activateDoor(false);
        currentEnemies = new List<GameObject>();
        waveNumber = 0;
        currentMaxWave = Mathf.RoundToInt(Mathf.Lerp((float)startWaveNum, (float)endWaveNum, GetComponent<LevelSwitching>().GetProgress()));
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        SpawnEnemyWave();
    }

    void ReshufflePoints(ref GameObject[] texts)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < texts.Length; t++)
        {
            GameObject tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
        }
    }

    public GameObject[] GetSpawnPoints()
    {
        return spawnPoints;
    }

    public void RegisterNewEnemy(GameObject enemy)
    {
        currentEnemies.Add(enemy);
        EnemiesLeftUpdater.enemiesLeft++;
        ShowIndicators();
    }

    public List<GameObject> GetCurrentEnemies()
    {
        return currentEnemies;
    }
}
