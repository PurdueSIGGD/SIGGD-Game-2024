using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        float waitTime = 4f;
        if (SceneManager.GetActiveScene().name.Contains("Cyberpunk"))
        {
            waitTime = 5f;
        }
        else if (SceneManager.GetActiveScene().name.Contains("Japan"))
        {
            waitTime = 4f;
        }
        else if (SceneManager.GetActiveScene().name.Contains("Medieval"))
        {
            waitTime = 4f;
        }

        yield return new WaitForSeconds(waitTime);
        if (SaveManager.data.orion < orionProgressTo) // do not let story progress go backwards, i.e. death convo must not repeat
        {
            SaveManager.data.orion = orionProgressTo;
        }
        Door.activateDoor(true);
        if (convo != null)
        {
            DialogueManager dialogueManager = FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include);
            dialogueManager.StartDialogue(convo);
        }
    }
}
