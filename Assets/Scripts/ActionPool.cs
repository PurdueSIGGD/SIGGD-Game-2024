using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionPool : MonoBehaviour
{
    public List<Action> actions;

    public ActionPool(List<Action> actions)
    {
        this.actions = actions;
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
}
