using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePoliceBasic : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("FireSidearm", null, SendMessageOptions.DontRequireReceiver);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopSidearm", null, SendMessageOptions.DontRequireReceiver);
    }
}
