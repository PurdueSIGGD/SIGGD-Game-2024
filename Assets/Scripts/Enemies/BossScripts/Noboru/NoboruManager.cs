using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NoboruManager : EnemyStateManager
{

    [Header("NOBORU PARAMS")]
    NoboruController controller;
    [SerializeField] GameObject tpSource;
    List<Transform> teleportPositions = new List<Transform>();
    Transform currentTransformPosition;
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
        Transform[] tpPositions = tpSource.GetComponentsInChildren<Transform>(includeInactive: false);
        teleportPositions = new(tpPositions);
        transform.position = teleportPositions[0].position;
        currentTransformPosition = teleportPositions[0];
    }
    public void Teleport()
    {
        List<Transform> transformsCopy = new(teleportPositions);
        transformsCopy.Remove(currentTransformPosition);

        int index = Random.Range(0, transformsCopy.Count);
        Transform tpPos = transformsCopy[index];

        transform.position = tpPos.position;
        currentTransformPosition = tpPos;
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
    public override bool HasLineOfSight(bool tracking)
    {
        // override L.O.S. calculation to be really super generous to the mage rather than require direct L.O.S.
        return Physics2D.OverlapCircle(summonTriggerSphere.transform.position, summonTriggerSphere.transform.lossyScale.x, LayerMask.GetMask("Player")) || base.HasLineOfSight(tracking);
    }
    public void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, summonTriggerSphere.lossyScale.x);
        Gizmos.DrawWireCube(tpTriggerBox.position, tpTriggerBox.lossyScale);
    }
}
