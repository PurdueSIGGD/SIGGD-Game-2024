using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterBossRoom : MonoBehaviour
{
    [SerializeField] EraDoor.Era era;
    private List<string> activeStoryBeatGhosts;

    void Start()
    {
        switch (era)
        {
            case EraDoor.Era.Cyberpunk:
                if (PartyManager.instance.IsGhostInParty("North-Police_Chief") && SaveManager.data.north.bossProgress == 0)
                {
                    activeStoryBeatGhosts.Add("North-Police_Chief");
                }
                if (PartyManager.instance.IsGhostInParty("Eva-Idol") && SaveManager.data.eva.bossProgress == 0)
                {
                    activeStoryBeatGhosts.Add("Eva-Idol");
                }
                break;
            case EraDoor.Era.Feudal:
                if (PartyManager.instance.IsGhostInParty("Akihito-Samurai") && SaveManager.data.akihito.bossProgress == 0)
                {
                    activeStoryBeatGhosts.Add("Akihito-Samurai");
                }
                if (PartyManager.instance.IsGhostInParty("Yume-Seamstress") && SaveManager.data.yume.bossProgress == 0)
                {
                    activeStoryBeatGhosts.Add("Yume-Seamstress");
                }
                break;
            case EraDoor.Era.Medieval:
                if (PartyManager.instance.IsGhostInParty("Silas-PlagueDoc") && SaveManager.data.silas.bossProgress == 0)
                {
                    activeStoryBeatGhosts.Add("Silas-PlagueDoc");
                }
                if (PartyManager.instance.IsGhostInParty("Aegis-King") && SaveManager.data.aegis.bossProgress == 0)
                {
                    activeStoryBeatGhosts.Add("Aegis-King");
                }
                break;
            default:
                Debug.LogWarning("no era set");
                break;
        }
        if (activeStoryBeatGhosts.Count == 0) return;
        
        string storyGhost = activeStoryBeatGhosts[Random.Range(0, activeStoryBeatGhosts.Count)];
    }
}
