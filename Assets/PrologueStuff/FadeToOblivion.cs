using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeToOblivion : MonoBehaviour
{
    private void OnEnable()
    {
        Door.OnDoorOpened += TransitionToOblivion;
    }

    private void OnDisable()
    {
        Door.OnDoorOpened -= TransitionToOblivion;
    }

    private void TransitionToOblivion()
    {
        SceneManager.LoadScene("Prologue_Hubworld");
    }
}
