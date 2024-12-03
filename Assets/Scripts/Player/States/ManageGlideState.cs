using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGlideState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StartGlide");
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopGlide");
    }

}
