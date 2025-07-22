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

    // Rough progression of story progress:
    // 0: first encounter
    // 1: Hub first entrance
    // 2: Story beat 1
    // 3: Story beat 2
    // 4: Story beat 3

    void DoorOpened()
    {
        // Cyberpunk story beats
        if (era == Era.Cyberpunk)
        {
            // North
            if (SaveManager.data.north.storyProgress == 0) // North First Encounter
            {
                SpecificLevelPool pool = new(new Level[] { new("North First Encounter", 1) }, 5);
                specificLevels.Add(pool);
            }
            else if (SaveManager.data.north.storyProgress == 2) // North Hub Dialogue
            {
                SpecificLevelPool pool = new(new Level[] { new("North Story Beat One", 1) }, Random.Range(1, 14));
                specificLevels.Add(pool);
            }
            else if (SaveManager.data.north.storyProgress == 3)
            {
                SpecificLevelPool pool = new(new Level[] { new("North Story Beat Two", 1) }, Random.Range(1, 14));
                specificLevels.Add(pool);
            }

            // Eva
            if (SaveManager.data.eva.storyProgress == 0) // Eva First Encounter
            {
                SpecificLevelPool pool = new(new Level[] { new("Eva First Encounter", 1) }, 5);
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
        Medival,
        Misc
    }
}
