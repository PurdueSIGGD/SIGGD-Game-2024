using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraDoor : MonoBehaviour
{
    [SerializeField] Level[] levels;
    [SerializeField] SpecificLevelPool[] specificLevels;
    [SerializeField] EnemySpawn[] enemies;
    void DoorOpened()
    {
        LevelSwitching.levels = levels;
        LevelSwitching.specificLevels = specificLevels;
        EnemySpawning.enemies = enemies;
    }
}
