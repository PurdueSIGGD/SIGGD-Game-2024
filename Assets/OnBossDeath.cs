using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBossDeath : MonoBehaviour
{
    [SerializeField] BossController bossToKill;
    [SerializeField] ConvoSO convo;


    private void OnEnable()
    {
        GameplayEventHolder.OnDeath += CheckBossDeath;
    }

    private void CheckBossDeath(DamageContext context)
    {
        if (context.victim == bossToKill.gameObject)
        {
            Door.activateDoor(true);
            if (convo)
            {
                DialogueManager dialogueManager = FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include);
                dialogueManager.StartDialogue(convo);
            }
        }
    }
}
