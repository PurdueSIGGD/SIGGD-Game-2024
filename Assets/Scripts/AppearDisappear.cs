using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// links a detector script with a gameobject
/// </summary>
public class AppearDisappear : MonoBehaviour
{
    //reference the object you want to hide or show
    public GameObject targetObject;

    /// <summary>
    /// sets the target object active
    /// </summary>
    public void ShowPlatform()
    {
        targetObject.SetActive(true);
    }
    /// <summary>
    /// sets the target object inactive
    /// </summary>
    public void HidePlatform()
    {
        targetObject.SetActive(false);
    }

}
