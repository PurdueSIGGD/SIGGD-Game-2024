using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StoryProgresser : MonoBehaviour
{
    [SerializeField] string ghost;
    [SerializeField] int progressTo;

    void Awake()
    {
        Door.OnDoorOpened += ProgressStory;
    }

    void OnDisable()
    {
        Door.OnDoorOpened -= ProgressStory;
    }

    private void ProgressStory()
    {
        switch (ghost.ToLower())
        {
            case "north":
                SaveManager.data.north.storyProgress = progressTo;
                break;
            case "eva":
                SaveManager.data.eva.storyProgress = progressTo;
                break;
            default:
                Debug.LogError("Can not recognize ghost: " + ghost.ToLower() + " when attempting to progress story");
                break;
        }
    }
}
