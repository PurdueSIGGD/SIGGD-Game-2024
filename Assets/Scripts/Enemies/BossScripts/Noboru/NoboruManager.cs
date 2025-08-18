using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoboruManager : EnemyStateManager
{

    [Header("NOBORU PARAMS")]
    NoboruController controller;
    List<Transform> teleportPositions = new List<Transform>();
    GameObject yokaiPrefab;
    [SerializeField] int numYokai;
    [SerializeField] List<GameObject> yokaiSpawnPool = new List<GameObject>();
    [SerializeField] float fancySummonInterval;
    public void Start()
    {
        base.Start();
    }
    public void Teleport()
    {
        int index = Random.Range(0, teleportPositions.Count);
        Vector2 tpPos = teleportPositions[index].position;
        transform.position = tpPos;
    }
    void SpawnYokai()
    {
        int index = Random.Range(0, yokaiSpawnPool.Count);
        GameObject enemyToSpawn = yokaiSpawnPool[index];
        controller.SpawnYokai(yokaiPrefab, enemyToSpawn);
    }
    public void SummonYokai()
    {
        StartCoroutine(FancySummonCoroutine());
    }
    IEnumerator FancySummonCoroutine()
    {
        for (int i = 0; i < numYokai; i++)
        {
            SpawnYokai();
            yield return new WaitForSeconds(fancySummonInterval);
        }
    }
}
