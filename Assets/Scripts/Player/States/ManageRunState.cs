using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageRunState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StartRun", null, SendMessageOptions.DontRequireReceiver);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopRun", null, SendMessageOptions.DontRequireReceiver);
        Debug.Log("Stop Runnig");
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("UpdateRun", null, SendMessageOptions.DontRequireReceiver);
    }

}
