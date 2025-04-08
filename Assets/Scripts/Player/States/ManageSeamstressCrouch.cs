using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSeamstressCrouchState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StartCrouch", null, SendMessageOptions.DontRequireReceiver);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopCrouch", null, SendMessageOptions.DontRequireReceiver);
        Debug.Log("Stop Crouching");
    }

}
