using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Enables communication between platform effect controllers and platform effect implementation.
/// </summary>
public interface ITriggerable
{
    //this interface is intended for platforms and game objects with repeatable, triggerable effects such as
    //a hazardous platform turning on and off on a constant timer, a block that dissapears when stepped on,
    //a button that can be pressed to toggle another platform, etc

    //this interface should be used with a general timer script or a general detector script that detects if a player is standing on top of an object, etc.

    //this is NOT for things like breakable pots, walls, etc, which should use a more general damageable interface

    /// <summary>
    /// sets the status of the effect to whether or not it should be active or not
    /// </summary>
    /// <param ghostName="active"></param>
    public void ToggleEffect(bool active);
    //if effect is one shot(like an explosion), you can either write the implementation in this method, or abstract it to a different method
    //if effect is persistent(ie. doing damage every tick like this one), you should implement the effect in a different loop, probably the update method or fixedupdate

    /// <summary>
    /// returns the value of if the effect is active or not
    /// </summary>
    /// <returns>bool</returns>
    public bool GetEffectStatus();
}
