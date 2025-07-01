using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingLoadingManager : MonoBehaviour
{
    private void Awake()
    {
        SaveManager.instance.Load();
    }

    // Start is called before the first frame update
    void Start()
    {
        Door.OnDoorOpened += OnDoorOpen;
    }

    void OnDoorOpen()
    {
        SaveManager.instance.Save();
    }
}
