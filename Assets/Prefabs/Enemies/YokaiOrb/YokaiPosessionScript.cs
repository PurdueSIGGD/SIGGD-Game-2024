using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YokaiPosessionScript : MonoBehaviour
{

    EnemySpawning enemySpawnManager;
    GameObject parentEnemyInstance;
    //[SerializeField] GameObject parentEnemyPrefab;
    [SerializeField] GameObject yokaiOrbPrefab;

    void OnEnable()
    {
        //GameplayEventHolder.OnDeath += OnDeath;
        GameplayEventHolder.OnDeathFilter.Add(OnDeathFilter);
    }
    void OnDisable()
    {
        //GameplayEventHolder.OnDeath -= OnDeath;
        GameplayEventHolder.OnDeathFilter.Remove(OnDeathFilter);
    }
    void Start()
    {
        enemySpawnManager = PersistentData.Instance.GetComponent<EnemySpawning>();
        parentEnemyInstance = this.transform.parent.gameObject;
        //print(parentEnemyPrefab.name);
    }
    public void OnDeathFilter(ref DamageContext damageContext)
    {
        if (damageContext.victim == parentEnemyInstance && !damageContext.damageTypes.Contains(DamageType.ENVIRONMENTAL))
        {
            AudioManager.Instance.SFXBranch.PlaySFXTrack("YokaiDeath");
            print("POSESSION VICTIM: " + damageContext.victim);
            //enemySpawnManager.SpawnEnemyWithDelay(this.transform.position, 0.1f, parentEnemyPrefab, yokaiOrbPrefab);
            GameObject nenemy = Instantiate(yokaiOrbPrefab, transform.position, transform.rotation);
            enemySpawnManager.RegisterNewEnemy(nenemy);
            //nenemy.GetComponent<YokaiOrbManager>().SetEnemyToSpawn(parentEnemyPrefab);
        }
    }
}
