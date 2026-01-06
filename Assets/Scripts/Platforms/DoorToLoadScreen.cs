using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToLoadScreen : Door
{
    [SerializeField] string loadingSceneName = "LoadingScreen";

    protected override void CallDoorOpened()
    {
        if (!transporting)
        {
            Door.activateDoor(false);
            transporting = true;
            SendMessage("DoorOpened");
            SceneManager.LoadScene(loadingSceneName, LoadSceneMode.Additive);
        }
    }
}
