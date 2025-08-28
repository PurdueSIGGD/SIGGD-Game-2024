using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterBossRoom : DialogueTriggerBox
{
    [SerializeField] EraDoor.Era era;
    [SerializeField] ConvoSO northBeforeConvo;
    [SerializeField] ConvoSO evaBeforeConvo;
    [SerializeField] ConvoSO akihitoBeforeConvo;
    [SerializeField] ConvoSO yumeBeforeConvo;
    [SerializeField] ConvoSO silasBeforeConvo;
    [SerializeField] ConvoSO aegisBeforeConvo;

    private List<ConvoSO> convos;
    private List<string> activeStoryBeatGhosts;

    void Start()
    {
        convos = new List<ConvoSO>();
        activeStoryBeatGhosts = new List<string>();
        switch (era)
        {
            case EraDoor.Era.Cyberpunk:
                if (PartyManager.instance.IsGhostInParty("North-Police_Chief") && SaveManager.data.north.bossProgress == 0)
                {
                    activeStoryBeatGhosts.Add("North-Police_Chief");
                    convos.Add(northBeforeConvo);
                }
                if (PartyManager.instance.IsGhostInParty("Eva-Idol") && SaveManager.data.eva.bossProgress == 0)
                {
                    activeStoryBeatGhosts.Add("Eva-Idol");
                    convos.Add(evaBeforeConvo);
                }
                break;
            case EraDoor.Era.Feudal:
                if (PartyManager.instance.IsGhostInParty("Akihito-Samurai") && SaveManager.data.akihito.bossProgress == 0)
                {
                    activeStoryBeatGhosts.Add("Akihito-Samurai");
                    convos.Add(akihitoBeforeConvo);
                }
                if (PartyManager.instance.IsGhostInParty("Yume-Seamstress") && SaveManager.data.yume.bossProgress == 0)
                {
                    activeStoryBeatGhosts.Add("Yume-Seamstress");
                    convos.Add(yumeBeforeConvo);
                }
                break;
            case EraDoor.Era.Medieval:
                if (PartyManager.instance.IsGhostInParty("Silas-PlagueDoc") && SaveManager.data.silas.bossProgress == 0)
                {
                    activeStoryBeatGhosts.Add("Silas-PlagueDoc");
                    convos.Add(silasBeforeConvo);
                }
                if (PartyManager.instance.IsGhostInParty("Aegis-King") && SaveManager.data.aegis.bossProgress == 0)
                {
                    activeStoryBeatGhosts.Add("Aegis-King");
                    convos.Add(aegisBeforeConvo);
                }
                break;
            default:
                Debug.LogWarning("no era set");
                break;
        }
        if (activeStoryBeatGhosts.Count == 0) return;
        
        int rand = Random.Range(0, activeStoryBeatGhosts.Count);
        string storyGhost = activeStoryBeatGhosts[rand];
        ConvoSO beforeBossConvo = convos[rand];

        active = true;
        SetConvo(beforeBossConvo);
    }
}
