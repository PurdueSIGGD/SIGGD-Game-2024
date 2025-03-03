using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageLightState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StartLightAttack", null, SendMessageOptions.DontRequireReceiver);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopLightAttack", null, SendMessageOptions.DontRequireReceiver);
    }
}