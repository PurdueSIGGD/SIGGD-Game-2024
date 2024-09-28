using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for ghost actions
/// </summary>
public abstract class GhostAction
{
    /// <summary>
    /// Called when the player switches to this major ghost
    /// </summary>
    public abstract void EnterSpecial();

    /// <summary>
    /// Called when the player switches from this major ghost to another
    /// </summary>
    public abstract void ExitSpecial();

    /// <summary>
    /// Called continuously by PartyManager's Update() while possessed by this ghost
    /// </summary>
    public abstract void UpdateSpecial();

    /// <summary>
    /// Called on special action
    /// </summary>
    /// <param name="context"></param>
    public abstract void OnSpecial(MonoBehaviour context);
}
