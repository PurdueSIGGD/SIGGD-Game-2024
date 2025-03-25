using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PersistentData is a GameObject that will be persistent across scenes and hold scripts for data
 * that needs to exist between scenes, like SpiritTracker.
 * 
 * PersistentData is following the Singleton pattern (and this script enforces it automatically):
 * 1. Only ONE instance of PersistentData can exist at any given time across all scenes.
 * 2. The PersistentData instance is accessible globally through the Instance property.
 * 
 * You can access scripts attached to PersistentData by doing:
 * PersistentData.Instance.GetComponent<YourScript>()
*/
public class PersistentData : MonoBehaviour
{
    public static PersistentData Instance {  get; private set; }   // allows read-only access to the PersistentData instance
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);   // destroys any duplicate instances of PersistentData
        }
    }
}
