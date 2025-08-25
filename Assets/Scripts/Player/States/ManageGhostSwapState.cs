using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGhostSwapState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("OPT");
        animator.SetTrigger("ATK");
        animator.SetTrigger("SPEC");
        animator.gameObject.SendMessage("StartGhostSwap", null, SendMessageOptions.DontRequireReceiver);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopGhostSwap", null, SendMessageOptions.DontRequireReceiver);
    }
}
