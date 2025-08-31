using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ManageIdleState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerID.instance.GetComponent<PartyManager>().SetSwappingEnabled(true);
        animator.ResetTrigger("OPT");
        animator.SetTrigger("ATK");
        animator.SetTrigger("SPEC");
        animator.gameObject.SendMessage("StartIdle", null, SendMessageOptions.DontRequireReceiver);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("StopIdle", null, SendMessageOptions.DontRequireReceiver);
    }
}
