using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawn
{
    [SerializeField] GameObject enemy;
    [SerializeField] float startChance;
    [SerializeField] float endChance;

    public float GetStartChance()
    {
        return startChance;
    }

    public float GetEndChance()
    {
        return endChance;
    }

    public GameObject GetEnemy()
    {
        return enemy;
    }
}
