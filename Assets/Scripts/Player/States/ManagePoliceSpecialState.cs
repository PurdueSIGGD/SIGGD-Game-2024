using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePoliceSpecialState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StartSpecialAttack", null, SendMessageOptions.DontRequireReceiver);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopSpecialAttack", null, SendMessageOptions.DontRequireReceiver);
    }
}