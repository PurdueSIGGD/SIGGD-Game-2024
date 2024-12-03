using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface connected to player glide scripts that lets the FSM determine how to toggle the gliding boolean;
/// </summary>
public interface IGlideMove
{
    /// <summary>
    /// Enables the player state machine to read whether or not the glide state should be entered,
    /// based on the glide script's current behavior.
    /// </summary>
    /// <returns>true if the player should be gliding, false if otherwise</returns>
    public bool GetBool();
}
