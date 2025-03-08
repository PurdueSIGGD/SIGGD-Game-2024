using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSkyLightState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StartSkyLightAttack", null, SendMessageOptions.DontRequireReceiver);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopSkyLightAttack", null, SendMessageOptions.DontRequireReceiver);
    }
}