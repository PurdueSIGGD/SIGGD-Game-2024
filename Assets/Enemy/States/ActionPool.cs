using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionPool
{
    public List<Action> actions;
    public Action move;
    public Action idle;

    public ActionPool(List<Action> actions, Action move, Action idle)
    {
        this.actions = actions;
        this.move = move;
        this.idle = idle;
    }

    public Action NextAction()
    {
        List<Action> avaliableActions = GetAvaliableActions();
        int count = avaliableActions.Count;
        if (count == 0)
        {
            return null;
        }
        Action nextAction = avaliableActions[0];
        for (int i = 0; i < count; i++)
        {
            // Randomize but take into account priority
        }
        return nextAction;
    }

    public List<Action> GetAvaliableActions()
    {
        List<Action> avaliableActions = new List<Action>();
        foreach (Action a in actions)
        {
            if (a.InAttackRange() & a.Ready())
            {
                avaliableActions.Add(a);
            }
        }
        return avaliableActions;
    }

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

    public void UpdateAllCD()
    {
        foreach (Action a in actions)
        {
            a.UpdateCD();
        }
    }
}
