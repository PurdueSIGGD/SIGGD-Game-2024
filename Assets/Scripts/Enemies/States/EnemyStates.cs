using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A state/mode of Enemy behavior
/// </summary>
public abstract class EnemyStates
{
    /// <summary>
    /// Behavior to be executed upon entering the state
    /// </summary>
    /// <param name="enemy"> reference to the enemy currently entering this state </param>
    public abstract void EnterState(EnemyStateManager enemy);

    /// <summary>
    /// Behavior to be checked/executed every frame
    /// </summary>
    /// <param name="enemy"> reference to the enemy currently in this state </param>
    public abstract void UpdateState(EnemyStateManager enemy);
}
