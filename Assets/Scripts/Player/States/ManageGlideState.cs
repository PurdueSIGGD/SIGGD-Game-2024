using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGlideState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StartGlide", null, SendMessageOptions.DontRequireReceiver);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopGlide", null, SendMessageOptions.DontRequireReceiver);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("UpdateGlide", null, SendMessageOptions.DontRequireReceiver);
    }

}
