using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageLightState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StartLightAttack");
        Debug.Log("XXX - IN Light Attack State!");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopLightAttack");
    }
}