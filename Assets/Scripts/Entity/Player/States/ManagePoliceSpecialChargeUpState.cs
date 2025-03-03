using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePoliceSpecialChargeUpState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StartSpecialChargeUp", null, SendMessageOptions.DontRequireReceiver);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopSpecialChargeUp", null, SendMessageOptions.DontRequireReceiver);
    }
}