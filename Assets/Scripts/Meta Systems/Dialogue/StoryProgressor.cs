using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StoryProgresser : MonoBehaviour
{
    [SerializeField] string requiredDialogue;
    [SerializeField] string ghost;
    [SerializeField] int progressTo;

    void Awake()
    {
        DialogueManager.onFinishDialogue += ProgressStory;
    }

    void OnDisable()
    {
        DialogueManager.onFinishDialogue -= ProgressStory;
    }

    public void Init(string requiredDialogue, string ghost, int progressTo)
    {
        this.requiredDialogue = requiredDialogue;
        this.ghost = ghost;
        this.progressTo = progressTo;
    }

    private void ProgressStory(string key)
    {
        if (!key.Equals(requiredDialogue)) return;

        switch (ghost.ToLower())
        {
            case "north":
                SaveManager.data.north.storyProgress = progressTo;
                break;
            case "eva":
                SaveManager.data.eva.storyProgress = progressTo;
                break;
            case "yume":
                SaveManager.data.yume.storyProgress = progressTo;
                break;
            case "akihito":
                SaveManager.data.akihito.storyProgress = progressTo;
                break;
            case "silas":
                SaveManager.data.silas.storyProgress = progressTo;
                break;
            case "aegis":
                SaveManager.data.aegis.storyProgress = progressTo;
                break;
            default:
                Debug.LogError("Can not recognize ghost: " + ghost.ToLower() + " when attempting to progress story");
                break;
        }
    }
}
