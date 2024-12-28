using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Greedy version of the Action Pool.  
/// Enemy Drawing from this pool will prioritize using attacks that are not under cool down.
/// </summary>
public class GreedyActionPool : ActionPool
{
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
