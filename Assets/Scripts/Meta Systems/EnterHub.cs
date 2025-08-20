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
    [SerializeField] ConvoSO northMaxTrust;

    [Header("Eva")]
    [SerializeField] GhostInteract eva;
    [SerializeField] ConvoSO evaHubEntrance;
    [SerializeField] ConvoSO evaMaxTrust;

    [Header("Akihito")]
    [SerializeField] GhostInteract akihito;
    [SerializeField] ConvoSO akihitoHubEntrance;
    [SerializeField] ConvoSO akihitoMaxTrust;

    [Header("Yume")]
    [SerializeField] GhostInteract yume;
    [SerializeField] ConvoSO yumeHubEntrance;
    [SerializeField] ConvoSO yumeMaxTrust;

    [Header("Silas")]
    [SerializeField] GhostInteract silas;
    [SerializeField] ConvoSO silasHubEntrance;
    [SerializeField] ConvoSO silasMaxTrust;

    [Header("Aegis")]
    [SerializeField] GhostInteract aegis;
    [SerializeField] ConvoSO aegisHubEntrance;
    [SerializeField] ConvoSO aegisMaxTrust;

    void Awake()
    {
        SaveManager.data.eva.tempoCount = 0;
        SaveManager.data.eva.remainingTempoDuration = 0f;
        SaveManager.data.yume.spoolCount = 0;
        SaveManager.data.aegis.damageDealtTillSmite = 0.0f;
        SaveManager.data.aegis.damageBlockTillSmite = 0.0f;
    }

    void Start()
    {
        // North
        if (SaveManager.data.north.storyProgress == 0)
        {
            north.gameObject.SetActive(false);
        }
        else
        {
            north.GetComponent<GhostIdentity>().UnlockGhost();
        }
        // load North on hub enter convo
        if (SaveManager.data.north.storyProgress == 1)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(northHubEntrance.data.convoName, "north", 2, true);
            north.SetConvo(northHubEntrance);
        }
        // load North max trust convo
        if (SaveManager.data.north.storyProgress == 4)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(northMaxTrust.data.convoName, "north", 5, false);
            north.SetConvo(northMaxTrust);
        }

        // Eva
        if (SaveManager.data.eva.storyProgress == 0)
        {
            eva.gameObject.SetActive(false);
        }
        else
        {
            eva.GetComponent<GhostIdentity>().UnlockGhost();
        }
        // load Eva on hub enter convo
        if (SaveManager.data.eva.storyProgress == 1)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(evaHubEntrance.data.convoName, "eva", 2, true);
            eva.SetConvo(evaHubEntrance);
        }
        // load Eva max trust convo
        if (SaveManager.data.eva.storyProgress == 4)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(evaMaxTrust.data.convoName, "eva", 5, false);
            eva.SetConvo(evaMaxTrust);
        }

        // Akihito
        if (SaveManager.data.akihito.storyProgress == 0)
        {
            akihito.gameObject.SetActive(false);
        }
        else
        {
            akihito.GetComponent<GhostIdentity>().UnlockGhost();
        }
        // load Akihito on hub enter convo
        if (SaveManager.data.akihito.storyProgress == 1)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(akihitoHubEntrance.data.convoName, "akihito", 2, true);
            akihito.SetConvo(akihitoHubEntrance);
        }
        // load Akihito max trust convo
        if (SaveManager.data.akihito.storyProgress == 4)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(akihitoMaxTrust.data.convoName, "akihito", 5, false);
            akihito.SetConvo(akihitoMaxTrust);
        }

        // Yume
        if (SaveManager.data.yume.storyProgress == 0)
        {
            yume.gameObject.SetActive(false);
        }
        else
        {
            yume.GetComponent<GhostIdentity>().UnlockGhost();
        }
        // load Yume on hub enter convo
        if (SaveManager.data.yume.storyProgress == 1)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(yumeHubEntrance.data.convoName, "yume", 2, true);
            yume.SetConvo(yumeHubEntrance);
        }
        // load Yume max trust convo
        if (SaveManager.data.yume.storyProgress == 4)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(yumeMaxTrust.data.convoName, "yume", 5, false);
            yume.SetConvo(yumeMaxTrust);
        }

        // Silas
        if (SaveManager.data.silas.storyProgress == 0)
        {
            silas.gameObject.SetActive(false);
        }
        else
        {
            silas.GetComponent<GhostIdentity>().UnlockGhost();
        }
        // load Silas on hub enter convo
        if (SaveManager.data.silas.storyProgress == 1)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(silasHubEntrance.data.convoName, "silas", 2, true);
            silas.SetConvo(silasHubEntrance);
        }
        // load Silas max trust convo
        if (SaveManager.data.silas.storyProgress == 4)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(silasMaxTrust.data.convoName, "silas", 5, false);
            silas.SetConvo(silasMaxTrust);
        }

        // Aegis
        if (SaveManager.data.aegis.storyProgress == 0)
        {
            aegis.gameObject.SetActive(false);
        }
        else
        {
            aegis.GetComponent<GhostIdentity>().UnlockGhost();
        }
        // load Aegis on hub enter convo
        if (SaveManager.data.aegis.storyProgress == 1)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(aegisHubEntrance.data.convoName, "aegis", 2, true);
            aegis.SetConvo(aegisHubEntrance);
        }
        // load Aegis max trust convo
        if (SaveManager.data.aegis.storyProgress == 4)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(aegisMaxTrust.data.convoName, "aegis", 5, false);
            aegis.SetConvo(aegisMaxTrust);
        }
    }
}
