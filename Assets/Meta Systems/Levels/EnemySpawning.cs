using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    public static EnemySpawning instance;
    public static EnemySpawn[] enemies;
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
    private GameObject[] points;
    
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

    public void OnDeath(DamageContext context)
    {
        if (currentEnemies.Contains(context.victim))
        {
            Debug.Log(context.victim.name+" has died");
            currentEnemies.Remove(context.victim);
            EnemiesLeftUpdater.enemiesLeft = currentEnemies.Count;
            if (currentEnemies.Count <= 0)
            {
                if(waveNumber + 1 < currentMaxWave)
                {
                    SpawnEnemies();
                    waveNumber += 1;
                }
                else
                {
                    Door.activateDoor(true);
                    foreach (Door door in GameObject.FindObjectsOfType<Door>()) 
                    {
                        Instantiate(doorIndicator, door.transform.position, Quaternion.identity);
                    }
                    //Active Door
                }
            }
        }

        if (context.victim.gameObject.CompareTag("Player"))
        {
            EnemiesLeftUpdater.enemiesLeft = -1;
        }

        ShowIndicators();
    }

    private GameObject GetNextEnemy()
    {
        float totalChance = 0.0f;
        for(int i = 0; i < enemies.Length; i++)
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

    private void SpawnEnemies()
    {
        reshufflePoints(ref points);
        int enemiesSpawn = Mathf.FloorToInt(Random.Range(Mathf.Lerp(startMinEnemiesSpawn, endMinEnemiesSpawn, GetComponent<LevelSwitching>().GetProgress()), Mathf.Lerp(startMaxEnemiesSpawn, endMaxEnemiesSpawn, GetComponent<LevelSwitching>().GetProgress())));
        //EnemiesLeftUpdater.enemiesLeft = enemiesSpawn;
        for (int i = 0; i < Mathf.Min(enemiesSpawn, points.Length); i++)
        {
            GameObject newEnemy = Instantiate(GetNextEnemy());
            currentEnemies.Add(newEnemy);
            newEnemy.transform.position = points[i].transform.position;
        }
        EnemiesLeftUpdater.enemiesLeft = currentEnemies.Count;
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
        points = GameObject.FindGameObjectsWithTag("SpawnPoint");
        SpawnEnemies();
    }

    void reshufflePoints(ref GameObject[] texts)
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
        return points;
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
