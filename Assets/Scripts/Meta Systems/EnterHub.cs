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
    [SerializeField] ConvoSO northStartSB3;

    [Header("Eva")]
    [SerializeField] GhostInteract eva;
    [SerializeField] ConvoSO evaHubEntrance;
    [SerializeField] ConvoSO evaMaxTrust;
    [SerializeField] ConvoSO evaStartSB3;

    [Header("Akihito")]
    [SerializeField] GhostInteract akihito;
    [SerializeField] ConvoSO akihitoHubEntrance;
    [SerializeField] ConvoSO akihitoMaxTrust;
    [SerializeField] ConvoSO akihitoStartSB3;

    [Header("Yume")]
    [SerializeField] GhostInteract yume;
    [SerializeField] ConvoSO yumeHubEntrance;
    [SerializeField] ConvoSO yumeMaxTrust;
    [SerializeField] ConvoSO yumeStartSB3;

    [Header("Silas")]
    [SerializeField] GhostInteract silas;
    [SerializeField] ConvoSO silasHubEntrance;
    [SerializeField] ConvoSO silasMaxTrust;
    [SerializeField] ConvoSO silasStartSB3;

    [Header("Aegis")]
    [SerializeField] GhostInteract aegis;
    [SerializeField] ConvoSO aegisHubEntrance;
    [SerializeField] ConvoSO aegisMaxTrust;
    [SerializeField] ConvoSO aegisStartSB3;

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
        // load North starting story beat 3
        if (SaveManager.data.north.storyProgress == 5 && SaveManager.data.ghostLevel["North-Police_Chief"] > 9)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(northStartSB3.data.convoName, "north", 6, false);
            north.SetConvo(northStartSB3);
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
        // load Eva starting story beat 3
        if (SaveManager.data.eva.storyProgress == 5 && SaveManager.data.ghostLevel["Eva-Idol"] > 9)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(evaStartSB3.data.convoName, "eva", 6, false);
            eva.SetConvo(evaStartSB3);
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
        // load Akihito starting story beat 3
        if (SaveManager.data.akihito.storyProgress == 5 && SaveManager.data.ghostLevel["Akihito-Samurai"] > 9)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(akihitoStartSB3.data.convoName, "akihito", 6, false);
            akihito.SetConvo(akihitoStartSB3);
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
        // load Yume starting story beat 3
        if (SaveManager.data.yume.storyProgress == 5 && SaveManager.data.ghostLevel["Yume-Seamstress"] > 9)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(yumeStartSB3.data.convoName, "yume", 6, false);
            yume.SetConvo(yumeStartSB3);
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
        // load Silas starting story beat 3
        if (SaveManager.data.silas.storyProgress == 5 && SaveManager.data.ghostLevel["Silas-PlagueDoc"] > 9)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(silasStartSB3.data.convoName, "silas", 6, false);
            silas.SetConvo(silasStartSB3);
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
        // load Aegis starting story beat 3
        if (SaveManager.data.aegis.storyProgress == 5 && SaveManager.data.ghostLevel["Aegis-King"] > 9)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(aegisStartSB3.data.convoName, "aegis", 6, false);
            aegis.SetConvo(aegisStartSB3);
        }
    }
}
