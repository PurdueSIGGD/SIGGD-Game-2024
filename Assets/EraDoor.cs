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
        First_Encounter = 0,
        Hub_First_Entrance = 1,
        Story_Beat_1 = 2,
        Story_Beat_2 = 3,
    }

    void DoorOpened()
    {
        InjectStoryBeat();
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
                ChooseStoryBeats(SaveManager.data.north, "North-Police_Chief", true);
                ChooseStoryBeats(SaveManager.data.eva, "Eva-Idol", false);
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

    private void ChooseStoryBeats(GhostData data, string name, bool isGhostOne)
    {
        int firstEncounterLoc = isGhostOne ? ghostOneFirstEncounterLoc : ghostTwoFirstEncounterLoc;
        int storyBeatOneLoc = isGhostOne ? ghostOneStoryBeatOne : ghostTwoStoryBeatOne;
        int storyBeatTwoLoc = isGhostOne ? ghostOneStoryBeatTwo : ghostTwoStoryBeatTwo;


        string truncName = name.Split('-')[0];
        
        if (data.storyProgress == (int)storyProgression.First_Encounter) // first encounter
        {
            specificLevels.Add(new(new Level[] { new(truncName + " First Encounter", 1) }, firstEncounterLoc));
            return;
        }
        else if (!PartyManager.instance.IsGhostInParty(name)) // if not time for first encounter and not in party
        {
            // inject the standard boss room and return
            specificLevels.Add(new(new Level[] { new("Cyberpunk_Boss", 1) },
                PersistentData.Instance.GetComponent<LevelSwitching>().GetMaxLevels()));
            return;
        }

        if (data.bossProgress == 0)
        {
            // inject ghost specific boss room
            specificLevels.Add(new(new Level[] { new(truncName + " Cyberpunk_Boss", 1) },
                PersistentData.Instance.GetComponent<LevelSwitching>().GetMaxLevels()));
        }
        else
        {
            // inject standard boss room
            specificLevels.Add(new(new Level[] { new("Cyberpunk_Boss", 1) },
                PersistentData.Instance.GetComponent<LevelSwitching>().GetMaxLevels()));
        }

        if ((data.storyProgress == (int)storyProgression.Hub_First_Entrance || // story beat 1
             data.storyProgress == (int)storyProgression.Story_Beat_1) &&
             SaveManager.data.ghostLevel[name] >= 3)
        {
            specificLevels.Add(new(new Level[] { new(truncName + " Story Beat One", 1) }, storyBeatOneLoc));
        }
        if (data.storyProgress == (int)storyProgression.Story_Beat_2 && // story beat 2
            SaveManager.data.ghostLevel[name] >= 6)
        {
            specificLevels.Add(new(new Level[] { new(truncName + " Story Beat Two", 1) }, storyBeatTwoLoc));
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
