using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Epilogue : MonoBehaviour
{
    [SerializeField] ConvoSO convo;
    [SerializeField] string moveTo;

    private void Start()
    {
        StartCoroutine(DelayedStart());
        DialogueManager.onFinishDialogue += DoTheThing;
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.1f);
        DialogueManager dialogueManager = FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include);
        dialogueManager.StartDialogue(convo);
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
        ScreenFader.instance.FadeOut(2, 3);
        yield return new WaitForSeconds(ScreenFader.instance.fadeOutDuration + 0.1f);
        SceneManager.LoadScene(moveTo);
    }
}
