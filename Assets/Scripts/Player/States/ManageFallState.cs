using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageFallState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StartFall", null, SendMessageOptions.DontRequireReceiver);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopFall", null, SendMessageOptions.DontRequireReceiver);
    }
}
