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
    //[SerializeField] int maxLevels = 20;
    //[SerializeField] int minPossibleStorybeatLoc = 3;
    //[SerializeField] int maxPossibleStorybeatLoc = 15;
    [SerializeField] int ghostOneFirstEncounterLoc = 3;
    [SerializeField] int ghostOneStoryBeatOne = 3;
    [SerializeField] int ghostOneStoryBeatTwo = 3;
    [SerializeField] int ghostTwoFirstEncounterLoc = 6;
    [SerializeField] int ghostTwoStoryBeatOne = 6;
    [SerializeField] int ghostTwoStoryBeatTwo = 6;

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
            else if (PartyManager.instance.IsGhostInParty("North-Police_Chief"))
            {
                if (SaveManager.data.north.storyProgress == 1 || SaveManager.data.north.storyProgress == 2) // North story beat 1
                {
                    SpecificLevelPool pool = new(new Level[] { new("North Story Beat One", 1) }, ghostOneStoryBeatOne);
                    specificLevels.Add(pool);
                }
                else if (SaveManager.data.north.storyProgress == 3) // North story beat 2
                {
                    SpecificLevelPool pool = new(new Level[] { new("North Story Beat Two", 1) }, ghostOneStoryBeatTwo);
                    specificLevels.Add(pool);
                }
            }

            // Eva
            if (SaveManager.data.eva.storyProgress == 0) // Eva First Encounter
            {
                SpecificLevelPool pool = new(new Level[] { new("Eva First Encounter", 1) }, ghostTwoFirstEncounterLoc);
                specificLevels.Add(pool);
            }
            else if (PartyManager.instance.IsGhostInParty("Eva-Idol"))
            {
                if (SaveManager.data.eva.storyProgress == 1 || SaveManager.data.eva.storyProgress == 2) // Eva story beat 1
                {
                    SpecificLevelPool pool = new(new Level[] { new("Eva Story Beat One", 1) }, ghostTwoStoryBeatOne);
                    specificLevels.Add(pool);
                }
                else if (SaveManager.data.eva.storyProgress == 3) // Eva story beat 2
                {
                    SpecificLevelPool pool = new(new Level[] { new("Eva Story Beat Two", 1) }, ghostTwoStoryBeatTwo);
                    specificLevels.Add(pool);
                }
            }
        }
        // Japan story beats
        else if (era == Era.Feudal)
        {
            // Yume
            if (SaveManager.data.yume.storyProgress == 0) // Yume first encounter
            {
                SpecificLevelPool pool = new(new Level[] { new("Yume First Encounter", 1) }, ghostOneFirstEncounterLoc);
                specificLevels.Add(pool);
            }
            else if (PartyManager.instance.IsGhostInParty("Yume-Seamstress"))
            {
                if (SaveManager.data.yume.storyProgress == 1 || SaveManager.data.yume.storyProgress == 2) // Yume story beat 1
                {
                    SpecificLevelPool pool = new(new Level[] { new("Yume Story Beat One", 1) }, ghostOneStoryBeatOne);
                    specificLevels.Add(pool);
                }
                else if (SaveManager.data.yume.storyProgress == 3) // Yume story beat 2
                {
                    SpecificLevelPool pool = new(new Level[] { new("Yume Story Beat Two", 1) }, ghostOneStoryBeatTwo);
                    specificLevels.Add(pool);
                }
            }

            // Akihito
            if (SaveManager.data.akihito.storyProgress == 0) // Akihito First Encounter
            {
                SpecificLevelPool pool = new(new Level[] { new("Akihito First Encounter", 1) }, ghostTwoFirstEncounterLoc);
                specificLevels.Add(pool);
            }
            else if (PartyManager.instance.IsGhostInParty("Akihito-Samurai"))
            {
                if (SaveManager.data.akihito.storyProgress == 1 || SaveManager.data.akihito.storyProgress == 2) // Akihito story beat 1
                {
                    SpecificLevelPool pool = new(new Level[] { new("Akihito Story Beat One", 1) }, ghostTwoStoryBeatOne);
                    specificLevels.Add(pool);
                }
                else if (SaveManager.data.akihito.storyProgress == 3) // Akihito story beat 2
                {
                    SpecificLevelPool pool = new(new Level[] { new("Akihito Story Beat Two", 1) }, ghostTwoStoryBeatTwo);
                    specificLevels.Add(pool);
                }
            }
        }
        // Medieval story beats
        else if (era == Era.Medieval)
        {
            // Silas
            if (SaveManager.data.silas.storyProgress == 0) // Silas first encounter
            {
                SpecificLevelPool pool = new(new Level[] { new("Silas First Encounter", 1) }, ghostOneFirstEncounterLoc);
                specificLevels.Add(pool);
            }
            else if (PartyManager.instance.IsGhostInParty("Silas-Plague_Doctor"))
            {
                if (SaveManager.data.silas.storyProgress == 1 || SaveManager.data.silas.storyProgress == 2) // Silas story beat 1
                {
                    SpecificLevelPool pool = new(new Level[] { new("Silas Story Beat One", 1) }, ghostOneStoryBeatOne);
                    specificLevels.Add(pool);
                }
                else if (SaveManager.data.silas.storyProgress == 3) // Silas story beat 2
                {
                    SpecificLevelPool pool = new(new Level[] { new("Silas Story Beat Two", 1) }, ghostOneStoryBeatTwo);
                    specificLevels.Add(pool);
                }
            }

            // Aegis
            if (SaveManager.data.aegis.storyProgress == 0) // Aegis First Encounter
            {
                SpecificLevelPool pool = new(new Level[] { new("Aegis First Encounter", 1) }, ghostTwoFirstEncounterLoc);
                specificLevels.Add(pool);
            }
            else if (PartyManager.instance.IsGhostInParty("Aegis-Samurai"))
            {
                if (SaveManager.data.aegis.storyProgress == 1 || SaveManager.data.aegis.storyProgress == 2) // Aegis story beat 1
                {
                    SpecificLevelPool pool = new(new Level[] { new("Aegis Story Beat One", 1) }, ghostTwoStoryBeatOne);
                    specificLevels.Add(pool);
                }
                else if (SaveManager.data.aegis.storyProgress == 3) // Aegis story beat 2
                {
                    SpecificLevelPool pool = new(new Level[] { new("Aegis Story Beat Two", 1) }, ghostTwoStoryBeatTwo);
                    specificLevels.Add(pool);
                }
            }
        }

        LevelSwitching.levels = levels;
        LevelSwitching.specificLevels = specificLevels.ToArray();
        EnemySpawning.enemies = enemies;
    }

    // can be used to generate a random index for story beat to occur, currently not in use
    //private int GenerateStorybeatLocation()
    //{
    //    int storyBeatLoc = Random.Range(minPossibleStorybeatLoc, maxPossibleStorybeatLoc);
    //    while (storyBeatLoc == ghostOneFirstEncounterLoc || storyBeatLoc == ghostTwoFirstEncounterLoc)
    //    {
    //        storyBeatLoc = Random.Range(minPossibleStorybeatLoc, maxPossibleStorybeatLoc);
    //    }
    //    return storyBeatLoc;
    //}

    public enum Era
    {
        Cyberpunk,
        Feudal,
        Medieval,
        Misc
    }
}
