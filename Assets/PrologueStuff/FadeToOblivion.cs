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
        //ScreenFader.instance.FadeOut(0, 2);
        ScreenFader.instance.FadeOut(0, 1);
        ScreenFader.instance.FadeIn(5f, 1);
        //yield return new WaitForSeconds(4.5f);
        AudioManager.Instance.SFXBranch.PlaySFXTrack("HeavyAttack");
        AudioManager.Instance.VABranch.PlayVATrack("Aegis-King Significant Damage Taken");
        AudioManager.Instance.MusicBranch.CrossfadeTo(MusicTrackName.NULL, 1f);
        //yield return new WaitForSeconds(ScreenFader.instance.fadeOutDuration + 3.5f);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("MidPrologue");
    }
}
