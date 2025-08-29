using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBossDeath : MonoBehaviour
{
    [SerializeField] int orionProgressTo;
    [SerializeField] BossController bossToKill;
    [SerializeField] ConvoSO convo; // dialogue between orion and boss
    private string ghost;

    private void OnEnable()
    {
        GameplayEventHolder.OnDeath += CheckBossDeath;
    }



    private void CheckBossDeath(DamageContext context)
    {
        if (context.victim == bossToKill.gameObject)
        {
            StartCoroutine(DelayCheckBossDeath());
        }
    }
    
    private IEnumerator DelayCheckBossDeath()
    {
        yield return new WaitForSeconds(1.5f);
        SaveManager.data.orion = orionProgressTo;
        Door.activateDoor(true);
        if (convo)
        {
            DialogueManager dialogueManager = FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include);
            dialogueManager.StartDialogue(convo);
        }
    }
}
