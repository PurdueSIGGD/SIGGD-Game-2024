using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * Consult with the META SYSTEMS team for any doubt about how this works ;-;
 *
 * If you have an animation state that needs to have an entry sound effect this shit is for you
 */

public class PlaySoundExit_State : StateMachineBehaviour
{
    [SerializeField] private SoundManager_Enum sound_type;
    [SerializeField, Range(0, 1)] private float volume = 1f;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager_Script.PlaySound(sound_type, volume);
    }
}
