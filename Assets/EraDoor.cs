using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EraDoor : MonoBehaviour
{
    [SerializeField] Level[] levels;
    [SerializeField] List<SpecificLevelPool> specificLevels;
    [SerializeField] EnemySpawn[] enemies;

    //[SerializeField] int maxLevels = 20;
    //[SerializeField] int minPossibleStorybeatLoc = 3;
    //[SerializeField] int maxPossibleStorybeatLoc = 15;

    [SerializeField] Era era;

    [SerializeField] int ghostOneFirstEncounterLoc = 3;
    [SerializeField] int ghostOneStoryBeatOne = 3;
    [SerializeField] int ghostOneStoryBeatTwo = 3;
    [SerializeField] int ghostTwoFirstEncounterLoc = 6;
    [SerializeField] int ghostTwoStoryBeatOne = 6;
    [SerializeField] int ghostTwoStoryBeatTwo = 6;

    private const string NORTH_NAME = "North-Police_Chief";
    private const string EVA_NAME = "Eva-Idol";
    private const string AKIHITO_NAME = "Akihito-Samurai";
    private const string YUME_NAME = "Yume-Seamstress";
    private const string SILAS_NAME = "Silas-PlagueDoc";
    private const string AEGIS_NAME = "Aegis-King";


    // Rough progression of story progress:
    // 0: first encounter
    // 1: Hub first entrance (autoprogress)
    // 2: Story beat 1
    // 3: Story beat 2
    // 4: Max Trust
    // 5: Story beat 3

    private enum storyProgression
    {
        First_Encounter,
        Hub_First_Entrance,
        Story_Beat_1,
    }

    void DoorOpened()
    {
        // Cyberpunk story beats
        if (era == Era.Cyberpunk)
        {
            // North
            if (SaveManager.data.north.storyProgress == (int)storyProgression.First_Encounter) // North First Encounter
            {
                SpecificLevelPool pool = new(new Level[] { new("North First Encounter", 1) }, ghostOneFirstEncounterLoc);
                specificLevels.Add(pool);
            }
            else if (PartyManager.instance.IsGhostInParty("North-Police_Chief"))
            {
                if ((SaveManager.data.north.storyProgress == (int)storyProgression.Hub_First_Entrance 
                     || SaveManager.data.north.storyProgress == (int)storyProgression.Story_Beat_1)
                     && SaveManager.data.ghostLevel[NORTH_NAME] >= 4) // North story beat 1
                {
                    SpecificLevelPool pool = new(new Level[] { new("North Story Beat One", 1) }, ghostOneStoryBeatOne);
                    specificLevels.Add(pool);
                }
                else if (SaveManager.data.north.storyProgress == 3
                         && SaveManager.data.ghostLevel[NORTH_NAME] >= 7) // North story beat 2
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
                if ((SaveManager.data.eva.storyProgress == 1 || SaveManager.data.eva.storyProgress == 2)
                     && SaveManager.data.ghostLevel[EVA_NAME] >= 4) // Eva story beat 1
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

    private void InjectStoryBeat()
    {
        GhostData data1;
        GhostData data2;

        switch(era)
        {
            case Era.Cyberpunk:
                data1 = SaveManager.data.north;
                data2 = SaveManager.data.eva;
                break;
            case Era.Feudal:
                data1 = SaveManager.data.akihito;
                data2 = SaveManager.data.yume;
                break;
            case Era.Medieval:
                data1 = SaveManager.data.silas;
                data2 = SaveManager.data.aegis;
                break;
            default:
                return; // do not inject if Misc
        }
    }

    private void ChooseStoryBeats(GhostData data1, GhostData data2, string name1, string name2)
    {
        string trunc_name1 = name1.Split('-')[0];
        string trunc_name2 = name2.Split('-')[0];
        
        if (data1.storyProgress == (int)storyProgression.First_Encounter) // first encounter
        {
            specificLevels.Add(new(new Level[] { new(trunc_name1 + " First Encounter", 1) }, ghostOneFirstEncounterLoc));
        }
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
