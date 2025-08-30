using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] ConvoSO convo;
    [SerializeField] float bossActiveDelay = 1.5f;
    [SerializeField] BossController boss;

    private void OnEnable()
    {
        DialogueManager.onFinishDialogue += ActivateBoss;
    }

    private void OnDisable()
    {
        DialogueManager.onFinishDialogue -= ActivateBoss;
    }

    private void ActivateBoss(string key)
    {
        if (key.Equals(convo.data.convoName)) StartCoroutine(DelayStartBoss());
    }

    private IEnumerator DelayStartBoss()
    {
        yield return new WaitForSeconds(bossActiveDelay);
        boss.EnableAI();
    }
}
