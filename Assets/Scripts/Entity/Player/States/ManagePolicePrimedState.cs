using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePolicePrimedState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StartSpecialPrimed", null, SendMessageOptions.DontRequireReceiver);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopSpecialPrimed", null, SendMessageOptions.DontRequireReceiver);
    }
}