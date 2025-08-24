using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnGuardDeath : MonoBehaviour
{
    [SerializeField] string nextSceneName;
    [SerializeField] GameObject poorGuy;
    
    void OnEnable()
    {
        Door.OnDoorOpened += TransitionToNextScene;
    }

    private void OnDisable()
    {
        Door.OnDoorOpened -= TransitionToNextScene;
    }

    void Update()
    {
        if (poorGuy == null)
        {
            Door.activateDoor(true);
        }    
    }

    private void TransitionToNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
