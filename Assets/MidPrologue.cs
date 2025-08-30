using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MidPrologue : MonoBehaviour
{
    [SerializeField] ConvoSO convo;

    private void Start()
    {
        DialogueManager dialogueManager = FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include);
        dialogueManager.StartDialogue(convo);

        DialogueManager.onFinishDialogue += DoTheThing;
    }

    private void OnDisable()
    {
        DialogueManager.onFinishDialogue -= DoTheThing;
    }

    private void DoTheThing(string key)
    {
        StartCoroutine(ThenTheThing());
    }

    private IEnumerator ThenTheThing()
    {
        ScreenFader.instance.FadeOut();
        yield return new WaitForSeconds(ScreenFader.instance.fadeOutDuration + 0.1f);
        SceneManager.LoadScene("Prologue_HubWorld");
    }
}
