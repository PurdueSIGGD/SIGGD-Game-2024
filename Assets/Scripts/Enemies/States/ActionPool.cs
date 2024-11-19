using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A list of possible Actions an Enemy can take.
/// An Action Pool is used to randomly output an avaliable action.
/// </summary>
public class ActionPool
{
    private List<Action> actions;
    public Action move;
    public Action idle;

    private float curWeight = 0f;
    private System.Random random = new System.Random();

    /// <summary>
    /// Constructs a new Action Pool
    /// </summary>
    /// <param name="actions"> List of Attack Actions </param>
    /// <param name="move"> Action containing moving animation </param>
    /// <param name="idle"> Action containing idling animation </param>
    public ActionPool(List<Action> actions, Action move, Action idle)
    {
        this.actions = actions;
        this.move = move;
        this.idle = idle;
    }

    /// <summary>
    /// Choose an action randomly out of all the currently avaliable actions
    /// </summary>
    /// <returns> the next action to be played </returns>
    public Action NextAction()
    {
        List<Action> avaliableActions = GetAvaliableActions();
        int count = avaliableActions.Count;
        if (count == 0)
        {
            return null;
        }

        Action nextAction = avaliableActions[0];
        double r = random.NextDouble();
        foreach (Action action in avaliableActions)
        {
            r -= action.priority / curWeight;
            if (r <= 0)
            {
                nextAction = action;
                curWeight = 0;
                break;
            }
        }
        return nextAction;
    }

    /// <summary>
    /// Finds if any Actions in pool can reach the player
    /// </summary>
    /// <returns> true if Player is in attack range </returns>
    public bool HasActionsInRange()
    {
        foreach (Action a in actions)
        {
            if (a.InAttackRange())
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Updates the cool down time of all the Actions in pool
    /// </summary>
    public void UpdateAllCD()
    {
        foreach (Action a in actions)
        {
            a.UpdateCD();
        }
    }

    // Return a list of actions that is currently avaliable
    private List<Action> GetAvaliableActions()
    {
        List<Action> avaliableActions = new List<Action>();
        foreach (Action a in actions)
        {
            if (a.InAttackRange() & a.Ready())
            {
                avaliableActions.Add(a);
                curWeight += a.priority;
            }
        }
        return avaliableActions;
    }
}
