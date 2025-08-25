using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorProgressor : MonoBehaviour
{
    [SerializeField] string ghost;
    [SerializeField] int progressTo;

    void DoorOpened()
    {
        //if (!key.Equals(requiredDialogue) && !autoProgress) return;

        switch (ghost.ToLower())
        {
            case "death":
                SaveManager.data.death = progressTo;
                break;
            case "orion":
                SaveManager.data.orion = progressTo;
                break;
            case "north":
                SaveManager.data.north.storyProgress = progressTo;
                break;
            case "north boss":
                SaveManager.data.north.bossProgress = progressTo;
                break;
            case "eva":
                SaveManager.data.eva.storyProgress = progressTo;
                break;
            case "eva boss":
                SaveManager.data.eva.bossProgress = progressTo;
                break;
            case "yume":
                SaveManager.data.yume.storyProgress = progressTo;
                break;
            case "yume boss":
                SaveManager.data.yume.bossProgress = progressTo;
                break;
            case "akihito":
                SaveManager.data.akihito.storyProgress = progressTo;
                break;
            case "akihito boss":
                SaveManager.data.akihito.bossProgress = progressTo;
                break;
            case "silas":
                SaveManager.data.silas.storyProgress = progressTo;
                break;
            case "silas boss":
                SaveManager.data.silas.bossProgress = progressTo;
                break;
            case "aegis":
                SaveManager.data.aegis.storyProgress = progressTo;
                break;
            case "aegis boss":
                SaveManager.data.aegis.bossProgress = progressTo;
                break;
            default:
                Debug.LogError("Can not recognize ghost: " + ghost.ToLower() + " when attempting to progress story");
                break;
        }
        SaveManager.instance.Save();
    }
}
