using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface that allows the player state machine to read a boolean from the special move script 
/// to determine whether or not to activate the special boolean, because determining the behavior
/// of a unique special ability is very difficult without understanding the behavior of the script,
/// so this was the least intrusive solution in order to keep the most separation between
/// action scripts and the state machine.
/// </summary>
public interface ISpecialMove
{
    /// <summary>
    /// Enables the player state machine to read whether or not the special move state should be entered,
    /// based on the special move script's current behavior.
    /// </summary>
    /// <returns>true if the special move is currently active, false if the special move is not active</returns>
    public bool GetBool();
    /// <summary>
    /// Calls the associated function to start the special move action.
    /// </summary>
    /// <returns></returns>
    public void StartSpecial();
    /// <summary>
    /// Calls the associated function to end the special move action, if applicable.
    /// </summary>
    /// <returns></returns>
    public void EndSpecial();
}