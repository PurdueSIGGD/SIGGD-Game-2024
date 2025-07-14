using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EraDoor : MonoBehaviour
{
    [SerializeField] Level[] levels;
    [SerializeField] List<SpecificLevelPool> specificLevels;
    [SerializeField] EnemySpawn[] enemies;

    [SerializeField] Era era;

    void DoorOpened()
    {
        // Cyberpunk story beats
        if (era == Era.Cyberpunk)
        {
            if (!SaveManager.data.north.isUnlocked) // North First Encounter
            {
                SpecificLevelPool pool = new SpecificLevelPool(new Level[] { new Level("North First Encounter", 1) }, 5);
                specificLevels.Add(pool);
            } 
        }
        LevelSwitching.levels = levels;
        LevelSwitching.specificLevels = specificLevels.ToArray();
        EnemySpawning.enemies = enemies;
    }

    public enum Era
    {
        Cyberpunk,
        Feudal,
        Medival
    }
}
