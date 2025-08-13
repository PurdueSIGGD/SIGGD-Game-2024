using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is only used to clear specific values on entering hub world
/// </summary>
public class EnterHub : MonoBehaviour
{
    [Header("North")]
    [SerializeField] GhostInteract north;
    [SerializeField] ConvoSO northHubEntrance;

    [Header("Eva")]
    [SerializeField] GhostInteract eva;
    [SerializeField] ConvoSO evaHubEntrance;

    [Header("Akihito")]
    [SerializeField] GhostInteract akihito;
    [SerializeField] ConvoSO akihitoHubEntrance;

    [Header("Yume")]
    [SerializeField] GhostInteract yume;
    [SerializeField] ConvoSO yumeHubEntrance;

    [Header("Silas")]
    [SerializeField] GhostInteract silas;
    [SerializeField] ConvoSO silasHubEntrance;

    [Header("Aegis")]
    [SerializeField] GhostInteract aegis;
    [SerializeField] ConvoSO aegisHubEntrance;

    void Awake()
    {
        SaveManager.data.eva.tempoCount = 0;
        SaveManager.data.eva.remainingTempoDuration = 0f;
        SaveManager.data.yume.spoolCount = 0;
        SaveManager.data.aegis.damageDealtTillSmite = 0.0f;
        SaveManager.data.aegis.damageBlockTillSmite = 0.0f;

        PersistentData.Instance.GetComponent<SpiritTracker>().ClearSpirits();
        PersistentData.Instance.GetComponent<ItemInventory>().ReturnItemsToPool();
    }

    void Start()
    {
        // North
        if (SaveManager.data.north.storyProgress == 0)
        {
            north.gameObject.SetActive(false);
        }
        // load North on hub enter convo
        if (SaveManager.data.north.storyProgress == 1)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(northHubEntrance.data.convoName, "north", 2, true);
            north.SetConvo(northHubEntrance);
        }

        // Eva
        if (SaveManager.data.eva.storyProgress == 0)
        {
            eva.gameObject.SetActive(false);
        }
        // load Eva on hub enter convo
        if (SaveManager.data.eva.storyProgress == 1)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(evaHubEntrance.data.convoName, "eva", 2, true);
            eva.SetConvo(evaHubEntrance);
        }
        
        // Akihito
        if (SaveManager.data.akihito.storyProgress == 0)
        {
            akihito.gameObject.SetActive(false);
        }
        // load Yume on hub enter convo
        if (SaveManager.data.yume.storyProgress == 1)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(akihitoHubEntrance.data.convoName, "akihito", 2, true);
            akihito.SetConvo(akihitoHubEntrance);
        }

        // Yume
        if (SaveManager.data.yume.storyProgress == 0)
        {
            yume.gameObject.SetActive(false);
        }
        // load Yume on hub enter convo
        if (SaveManager.data.yume.storyProgress == 1)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(yumeHubEntrance.data.convoName, "yume", 2, true);
            yume.SetConvo(yumeHubEntrance);
        }

        // Silas
        if (SaveManager.data.silas.storyProgress == 0)
        {
            silas.gameObject.SetActive(false);
        }
        // load Yume on hub enter convo
        if (SaveManager.data.yume.storyProgress == 1)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(silasHubEntrance.data.convoName, "silas", 2, true);
            silas.SetConvo(silasHubEntrance);
        }

        // Aegis
        if (SaveManager.data.aegis.storyProgress == 0)
        {
            aegis.gameObject.SetActive(false);
        }
        // load Yume on hub enter convo
        if (SaveManager.data.aegis.storyProgress == 1)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(aegisHubEntrance.data.convoName, "aegis", 2, true);
            aegis.SetConvo(aegisHubEntrance);
        }
    }
}
