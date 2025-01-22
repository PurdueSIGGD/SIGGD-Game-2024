using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageHeavyState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StartHeavyAttack", null, SendMessageOptions.DontRequireReceiver);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopHeavyAttack", null, SendMessageOptions.DontRequireReceiver);
    }
}