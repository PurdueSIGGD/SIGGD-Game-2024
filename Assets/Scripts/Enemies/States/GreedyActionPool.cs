using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Greedy version of the Action Pool.  
/// Enemy Drawing from this pool will prioritize using attacks that are not under cool down.
/// </summary>
public class GreedyActionPool : ActionPool
{
    // It is strongly recomended for an enemy implmenting this AI to have an attack that is:
    // 1. Have no cool down.
    // 2. Can reach the player even if they are really close (e.g. melee)
    public GreedyActionPool(List<Action> actions, Action move, Action idle) : base(actions, move, idle) { }

    // Check if an action exists that both can reach the player and is not under cool down.
    public override bool HasActionsReady()
    {
        foreach (Action a in actions)
        {
            if (a.ready & a.InAttackRange())
            {
                return true;
            }
        }
        return false;
    }
}
