using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract class for ghost actions
public abstract class GhostAction
{
    // Called when the player switches to this major ghost
    public abstract void EnterSpecial();

    // Called when the player switches from this major ghost to another
    public abstract void ExitSpecial();

    // Called continuously by PartyManager's Update() while possessed by this ghost
    public abstract void UpdateSpecial();

    // Called on special action
    public abstract void OnSpecial(MonoBehaviour context);
}
