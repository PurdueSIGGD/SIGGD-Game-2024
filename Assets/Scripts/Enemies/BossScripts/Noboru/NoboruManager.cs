using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NoboruManager : EnemyStateManager
{

    [Header("NOBORU PARAMS")]
    NoboruController controller;
    [SerializeField] GameObject tpSource;
    [SerializeField] List<Transform> teleportPositions = new List<Transform>();
    [SerializeField] GameObject yokaiPrefab;
    [SerializeField] int numYokai;
    [SerializeField] List<GameObject> yokaiSpawnPool = new List<GameObject>();
    [SerializeField] float fancySummonInterval;
    [SerializeField] Transform tpTriggerBox;
    [SerializeField] Transform summonTriggerSphere;
    public void Start()
    {
        base.Start();
        controller = GetComponent<NoboruController>();
        Transform[] tpPositions = tpSource.GetComponentsInChildren<Transform>();
        teleportPositions = new(tpPositions);
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
    public void SummonYokaiWave()
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
    public void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, summonTriggerSphere.lossyScale.x);
        Gizmos.DrawWireCube(tpTriggerBox.position, tpTriggerBox.lossyScale);
    }
}
