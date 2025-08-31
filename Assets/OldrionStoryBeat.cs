using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldrionStoryBeat : MonoBehaviour
{
    [SerializeField] DialogueTriggerBox BeforeFightConvoTrigger;
    [SerializeField] ConvoSO aegisAlternativeBeforeConvo;


    void Start()
    {
        if (PartyManager.instance.IsGhostInParty("Aegis-King"))
        {
            BeforeFightConvoTrigger.SetConvo(aegisAlternativeBeforeConvo);
        }
    }
}
