using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageHeavyChargeUpState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StartHeavyChargeUp", null, SendMessageOptions.DontRequireReceiver);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopHeavyChargeUp", null, SendMessageOptions.DontRequireReceiver);
    }
}