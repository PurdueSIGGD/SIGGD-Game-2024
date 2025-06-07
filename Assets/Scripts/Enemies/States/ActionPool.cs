using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A list of possible Actions an Enemy can take.
/// An Action Pool is used to randomly output an available action.
/// </summary>
public class ActionPool : MonoBehaviour
{
    [SerializeField] protected List<Action> actions;
    public Action move;
    public Action idle;
    
    private float curWeight = 0f;

    void Start()
    {
        foreach (Action a in actions)
        {
            //a.ready = true; // default each action is ready on start, subject to change
            if (a.priority == 0)
            {
                Debug.LogWarning(a.name + " should have a non-zero priority."); 
            }
        }
    }

    /// <summary>
    /// Choose an action randomly out of all the currently available actions
    /// </summary>
    /// <returns> the next action to be played </returns>
    public Action NextAction()
    {
        List<Action> availableActions = GetAvailableActions();
        if (availableActions.Count == 0) { return null; }

        Action nextAction = availableActions[0];
        double r = Random.value;
        foreach (Action action in availableActions)
        {
            r -= action.GetPriority() / curWeight;
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
    public virtual bool HasActionsReady()
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

    // Return a list of actions that is currently available
    private List<Action> GetAvailableActions()
    {
        List<Action> availableActions = new List<Action>();
        foreach (Action a in actions)
        {
            if (a.InAttackRange() & a.ready)
            {
                availableActions.Add(a);
                curWeight += a.GetPriority();
            }
        }
        return availableActions;
    }
}
