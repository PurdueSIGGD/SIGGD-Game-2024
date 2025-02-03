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
    /// Calls the associated function to start the special move action.
    /// </summary>
    /// <returns></returns>
    public void StartSpecial();
}