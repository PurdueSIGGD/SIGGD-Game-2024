using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionPool
{
    public List<Action> actions;
    public Action move;
    public Action idle;

    private float curWeight = 0f;
    private System.Random random = new System.Random();

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
        double r = random.NextDouble();
        Debug.Log(r);
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

    public List<Action> GetAvaliableActions()
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
