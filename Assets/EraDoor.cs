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
    [SerializeField] int maxLevels = 20;
    [SerializeField] int ghostOneFirstEncounterLoc = 5;
    [SerializeField] int ghostTwoFirstEncounterLoc = 15;
    [SerializeField] int minPossibleStorybeatLoc = 3;
    [SerializeField] int maxPossibleStorybeatLoc = 15;

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
                SpecificLevelPool pool = new(new Level[] { new("North First Encounter", 1) }, ghostOneFirstEncounterLoc);
                specificLevels.Add(pool);
            }
            else if (SaveManager.data.north.storyProgress == 2) // North Hub Dialogue
            {
                SpecificLevelPool pool = new(new Level[] { new("North Story Beat One", 1) }, GenerateStorybeatLocation());
                specificLevels.Add(pool);
            }
            else if (SaveManager.data.north.storyProgress == 3)
            {
                SpecificLevelPool pool = new(new Level[] { new("North Story Beat Two", 1) }, GenerateStorybeatLocation());
                specificLevels.Add(pool);
            }

            // Eva
            if (SaveManager.data.eva.storyProgress == 0) // Eva First Encounter
            {
                SpecificLevelPool pool = new(new Level[] { new("Eva First Encounter", 1) }, ghostTwoFirstEncounterLoc);
                specificLevels.Add(pool);
            }
            else if (SaveManager.data.eva.storyProgress == 2) // North Hub Dialogue
            {
                SpecificLevelPool pool = new(new Level[] { new("Eva Story Beat One", 1) }, GenerateStorybeatLocation());
                specificLevels.Add(pool);
            }
            else if (SaveManager.data.eva.storyProgress == 3)
            {
                SpecificLevelPool pool = new(new Level[] { new("Eva Story Beat Two", 1) }, GenerateStorybeatLocation());
                specificLevels.Add(pool);
            }
        }
        LevelSwitching.levels = levels;
        LevelSwitching.specificLevels = specificLevels.ToArray();
        EnemySpawning.enemies = enemies;
    }

    private int GenerateStorybeatLocation()
    {
        int storyBeatLoc = Random.Range(minPossibleStorybeatLoc, maxPossibleStorybeatLoc);
        while (storyBeatLoc == ghostOneFirstEncounterLoc || storyBeatLoc == ghostTwoFirstEncounterLoc)
        {
            storyBeatLoc = Random.Range(minPossibleStorybeatLoc, maxPossibleStorybeatLoc);
        }
        return storyBeatLoc;
    }

    public enum Era
    {
        Cyberpunk,
        Feudal,
        Medival,
        Misc
    }
}
