using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageDashState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("OPT");
        animator.gameObject.SendMessage("StartDash", null, SendMessageOptions.DontRequireReceiver);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopDash", null, SendMessageOptions.DontRequireReceiver);
    }
}
