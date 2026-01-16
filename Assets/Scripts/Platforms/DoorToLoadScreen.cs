using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToLoadScreen : Door
{
    [SerializeField] string loadingSceneName = "LoadingScreen";

    EraDoor eraDoor;

    void Start()
    {
        eraDoor = GetComponent<EraDoor>();
    }

    public override void Teleport()
    {
        if (!transporting)
        {
            Door.activateDoor(false);
            transporting = true;
            if (eraDoor) eraDoor.teleporting = true;
            SendMessage("DoorOpened");
            SceneManager.LoadScene(loadingSceneName, LoadSceneMode.Additive);
        }
    }
}
