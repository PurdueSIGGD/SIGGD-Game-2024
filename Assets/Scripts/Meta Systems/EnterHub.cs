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
        // load North on hub enter convo
        if (SaveManager.data.north.storyProgress == 1)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(northHubEntrance.data.convoName, "north", 2);
            north.SetConvo(northHubEntrance);
        }

        // load Eva on hub enter convo
        if (SaveManager.data.eva.storyProgress == 1)
        {
            StoryProgresser sp = gameObject.AddComponent<StoryProgresser>();
            sp.Init(evaHubEntrance.data.convoName, "eva", 2);
            eva.SetConvo(evaHubEntrance);
        }
    }
}
