using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This script is only used to clear specific values on entering hub world
/// </summary>
public class EnterHub : MonoBehaviour
{
    [Header("Ghosts")]
    [SerializeField] List<GhostHubInfo> ghostHubInfos;

    [Header("Death")]
    [SerializeField] DialogueTriggerBox death;
    [SerializeField] ConvoSO deathFirstDeath;
    [SerializeField] ConvoSO deathNovaPoint;
    [SerializeField] ConvoSO deathShigora;
    [SerializeField] ConvoSO deathCaladria;

    [Header("Generic")]
    [SerializeField] GameObject newInteractionIndicator;

    [Header("Ghost To Ghost Convo")]
    [Tooltip("Out of 100"), SerializeField] float triggerChance;
    [SerializeField] GhostInteract g2gConvoInteractable;
    [SerializeField] SpriteRenderer[] g2gGhostIcons;
    [SerializeField] GhostToGhostConvoHolder[] ghostToGhostConvos;

    List<GhostHubInfo> avaliableGhostToGhostConvo;

    void Awake()
    {
        SaveManager.data.eva.tempoCount = 0;
        SaveManager.data.eva.remainingTempoDuration = 0f;
        SaveManager.data.yume.spoolCount = 0;
        SaveManager.data.aegis.damageDealtTillSmite = 0.0f;
        SaveManager.data.aegis.damageBlockTillSmite = 0.0f;

        PersistentData.Instance.GetComponent<SpiritTracker>().ClearSpirits();
        PersistentData.Instance.GetComponent<ItemInventory>().ReturnItemsToPool();

        avaliableGhostToGhostConvo = new();
    }

    void Start()
    {
        LoadDeathEncounter();
        LoadConversation();
        LoadGhostToGhost();
    }

    private void LoadGhostToGhost()
    {
        g2gConvoInteractable.gameObject.SetActive(false);
        int numGhostsAvaliable = avaliableGhostToGhostConvo.Count;
        if (numGhostsAvaliable < 2 || Random.Range(0, 100) >= triggerChance) return;


        int i, j;
        i = j = Random.Range(0, numGhostsAvaliable);
        while (j == i) j = Random.Range(0, numGhostsAvaliable);
        int ghost1Index = GetGhostSaveData(avaliableGhostToGhostConvo[i].nickname).Index;
        int ghost2Index = GetGhostSaveData(avaliableGhostToGhostConvo[j].nickname).Index;
        if (ghost1Index > ghost2Index)
        {
            // swap so that lower index is always first, saves work in editor
            (ghost2Index, ghost1Index) = (ghost1Index, ghost2Index);
        }
        int relationshipLvl = SaveManager.data.ghostToGhostProgress[ghost1Index].row[ghost2Index];
        ConvoSO selectedConvo = relationshipLvl == 0 ?
            ghostToGhostConvos[ghost1Index].convoRow[ghost2Index].firstMeet :
            ghostToGhostConvos[ghost1Index].convoRow[ghost2Index].StdConvo;

        g2gConvoInteractable.gameObject.SetActive(true);
        g2gConvoInteractable.SetConvo(selectedConvo);

        SaveManager.data.ghostToGhostProgress[ghost1Index].row[ghost2Index] = 1;

        // hide the actual ghosts in scene
        avaliableGhostToGhostConvo[i].interact.gameObject.SetActive(false);
        avaliableGhostToGhostConvo[j].interact.gameObject.SetActive(false);
        g2gGhostIcons[0].color = avaliableGhostToGhostConvo[i].displayColor;
        g2gGhostIcons[1].color = avaliableGhostToGhostConvo[j].displayColor;



        // below is implementation if we don't want hub convos to repeat, which I changed my mind on last sec

        // pick a random index and search through the list from there, so to pick a random ghost convo
        //int i, startingIndex;
        //ConvoSO selectedConvo = null;
        //i = startingIndex = Random.Range(0, numGhostsAvaliable);
        //do
        //{
        //    for (int j = i + 1; j < numGhostsAvaliable; j ++)
        //    {
        //        if (i == j)
        //        {
        //            Debug.LogError("Impossible, shouldn't be checking same ghost against itself");
        //            continue;
        //        }

        //        int ghost1Index = GetGhostSaveData(avaliableGhostToGhostConvo[i]).Index;
        //        int ghost2Index = GetGhostSaveData(avaliableGhostToGhostConvo[j]).Index;
        //        if (ghost2Index > ghost1Index)
        //        {
        //            // swap so that lower index is always first, saves work in editor
        //            (ghost2Index, ghost1Index) = (ghost1Index, ghost2Index);
        //        }

        //        int relationshipLvl = SaveManager.data.ghostToGhostProgress[ghost1Index][ghost2Index];
        //        //if (relationshipLvl == 3) continue;
        //        selectedConvo = relationshipLvl == 0 ?
        //            ghostToGhostConvos[ghost1Index].convoRow[ghost2Index].firstMeet :
        //            ghostToGhostConvos[ghost1Index].convoRow[ghost2Index].StdConvo;

        //        g2gConvoInteractable.gameObject.SetActive(true);
        //        g2gConvoInteractable.SetConvo(selectedConvo);
        //    }

        //    if (++i == numGhostsAvaliable) i = 0; // wrap around if reach end of list

        //} while (i != startingIndex);
    }

    private void LoadConversation()
    {
        foreach (GhostHubInfo ghostInfo in ghostHubInfos)
        {
            GhostData saveData = GetGhostSaveData(ghostInfo.nickname);
            GameObject ghost = ghostInfo.interact.gameObject;
            string fullname = ghostInfo.fullname;
            string nickname = ghostInfo.nickname;
            GhostInteract interact = ghostInfo.interact;
            ConvoSO hubEntrance = ghostInfo.hubEntrance;
            ConvoSO maxTrust = ghostInfo.maxTrust;
            ConvoSO startSB3 = ghostInfo.startSB3;

            if (saveData.storyProgress == 0)
            {
                ghost.SetActive(false);
                continue;
            }
            else
            {
                ghost.GetComponent<GhostIdentity>().UnlockGhost();
            }

            // on hub enter convo
            if (saveData.storyProgress == 1)
            {
                StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
                sp.Init(hubEntrance.data.convoName, nickname, 2, true);
                interact.SetConvo(hubEntrance, newInteractionIndicator);
                continue;
            }
            // max trust convo
            if (saveData.storyProgress == 4 && SaveManager.data.ghostLevel[fullname] >= 9)
            {
                StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
                sp.Init(maxTrust.data.convoName, nickname, 5, false);
                interact.SetConvo(maxTrust, newInteractionIndicator);
                continue;
            }
            // starting story beat 3
            if (saveData.storyProgress == 5 && SaveManager.data.ghostLevel[fullname] >= 11)
            {
                StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
                sp.Init(startSB3.data.convoName, nickname, 6, false);
                interact.SetConvo(startSB3, newInteractionIndicator);
                continue;
            }

            avaliableGhostToGhostConvo.Add(ghostInfo);
        }
    }

    private void LoadDeathEncounter()
    {
        // orion = 0: new game
        // orion = 1: beated nova point
        // orion = 2: talked to death about nova point
        // orion = 3: beated shigora
        // orion = 4: talked to death about shigora
        // orion = 5: beated caladria
        // orion = 6: talked to death about caladria

        if (death && SaveManager.data.death == 1) // First Death encounter
        {
            death.gameObject.SetActive(true);
            death.SetConvo(deathFirstDeath);
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(deathFirstDeath.data.convoName, "death", 2, true);
        }
        if (death && SaveManager.data.orion == 1) // beating nova point
        {
            death.gameObject.SetActive(true);
            death.SetConvo(deathNovaPoint);
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(deathNovaPoint.data.convoName, "orion", 2, true);
        }
        if (death && SaveManager.data.orion == 3) // beating shigora
        {
            death.gameObject.SetActive(true);
            death.SetConvo(deathShigora);
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(deathShigora.data.convoName, "orion", 4, true);
        }
        if (death && SaveManager.data.orion == 5) // beating caladria
        {
            death.gameObject.SetActive(true);
            death.SetConvo(deathCaladria);
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(deathCaladria.data.convoName, "orion", 6, true);
        }
    }

    private GhostData GetGhostSaveData(string name)
    {
        return name switch
        {
            "north" => SaveManager.data.north,
            "eva" => SaveManager.data.eva,
            "akihito" => SaveManager.data.akihito,
            "yume" => SaveManager.data.yume,
            "silas" => SaveManager.data.silas,
            "aegis" => SaveManager.data.aegis,
            _ => null
        };
    }
}

[Serializable]
struct GhostHubInfo
{
    public string fullname;
    public string nickname;
    public Color displayColor;
    public GhostInteract interact;
    public ConvoSO hubEntrance;
    public ConvoSO maxTrust;
    public ConvoSO startSB3;
}

// Separate class to bypass Unity's serialization limit of nested arrays
[Serializable]
class GhostToGhostConvoHolder
{
    public GhostToGhostConvo[] convoRow = new GhostToGhostConvo[6];
}

[Serializable]
class GhostToGhostConvo
{
    public ConvoSO firstMeet;
    [HideInInspector] public ConvoSO StdConvo => stdConvo[Random.Range(0, stdConvo.Length)];

    [SerializeField] ConvoSO[] stdConvo;
}
