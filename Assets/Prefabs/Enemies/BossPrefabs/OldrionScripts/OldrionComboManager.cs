using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldrionComboManager : MonoBehaviour
{
    ActionPool actionPool;
    [SerializeField] List<Combo> combos = new List<Combo>();
    Combo currentCombo;
    [SerializeField] bool inCombo;
    bool applyBuffer;
    [SerializeField] Action comboExitAction;
    [SerializeField] Action comboBufferAction;

    void Start()
    {
        actionPool = GetComponent<ActionPool>();
    }
    public void StartCombo()
    {
        currentCombo = GrabRandomCombo();
        inCombo = true;
        applyBuffer = false;
    }
    Combo GrabRandomCombo()
    {
        return new Combo(combos[Random.Range(0, combos.Count)]);
    }
    public Action GetNextAction()
    {
        if (applyBuffer)
        {
            applyBuffer = false;
            return comboBufferAction;
        }
        string nextActionName = currentCombo.NextActionName();
        if (nextActionName == null)
        {
            EndCombo();
            return comboExitAction;
        }
        applyBuffer = true;
        return actionPool.GetActionByName(nextActionName);
    }
    void EndCombo()
    {
        currentCombo = null;
        inCombo = false;
    }
}