using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeToOblivion : MonoBehaviour
{
    private void OnEnable()
    {
        DialogueManager.onFinishDialogue += TransitionToOblivion;
    }

    private void OnDisable()
    {
        DialogueManager.onFinishDialogue -= TransitionToOblivion;
    }

    private void TransitionToOblivion(string key)
    {
        StartCoroutine(DoTheThing());
    }

    private IEnumerator DoTheThing()
    {
        ScreenFader.instance.FadeOut();
        yield return new WaitForSeconds(ScreenFader.instance.fadeOutDuration + 0.1f);
        SceneManager.LoadScene("PrePrologue");
    }
}
