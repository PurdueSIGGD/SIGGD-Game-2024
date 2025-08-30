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
        ScreenFader.instance.FadeOut(0, 2);
        yield return new WaitForSeconds(4.5f);
        AudioManager.Instance.SFXBranch.PlaySFXTrack("HeavyAttack");
        yield return new WaitForSeconds(ScreenFader.instance.fadeOutDuration + 3.5f);
        SceneManager.LoadScene("MidPrologue");
    }
}
