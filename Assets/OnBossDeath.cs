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

    private void OnDisable()
    {
        GameplayEventHolder.OnDeath -= CheckBossDeath;
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
        if (SaveManager.data.orion < orionProgressTo) // do not let story progress go backwards, i.e. death convo must not repeat
        {
            SaveManager.data.orion = orionProgressTo;
        }
        Door.activateDoor(true);
        if (convo)
        {
            DialogueManager dialogueManager = FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include);
            dialogueManager.StartDialogue(convo);
        }
    }
}
