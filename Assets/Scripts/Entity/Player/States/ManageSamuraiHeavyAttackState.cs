using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSamuraiHeavyAttackState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StartSamuraiHeavyAttack", null, SendMessageOptions.DontRequireReceiver);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopSamuraiHeavyAttack", null, SendMessageOptions.DontRequireReceiver);
    }
}