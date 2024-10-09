using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Electric platform prefab script that periodically becomes SHOCKING!!! 
/// Implements the ITriggerable interface.
/// </summary>

public class ElectricPlatform : MonoBehaviour, ITriggerable
{

    bool isEffectActive; //is the special platform effect on or off?
    SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        //sprite to change color to indicate effect on or off, because player hurt implementation by player team not done yet
        if (isEffectActive)
        {
            sprite.color = Color.blue;
        }
        else
        {
            sprite.color = Color.gray;
        }
    }
    /// <summary>
    /// sets the status of the effect to whether or not it should be active or not
    /// </summary>
    /// <param name="active"></param>
    public void ToggleEffect(bool active) //ITriggerable method, mutator
    {
        isEffectActive = active;
    }
    /// <summary>
    /// returns the value of if the effect is active or not
    /// </summary>
    /// <returns>bool</returns>
    public bool GetEffectStatus() //ITriggerable method, accessor
    {
        return isEffectActive;
    }

}
