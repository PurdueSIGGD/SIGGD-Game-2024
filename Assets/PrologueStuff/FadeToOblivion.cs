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
        SceneManager.LoadScene("Prologue_Hubworld");
    }
}
