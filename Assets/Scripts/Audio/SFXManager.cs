using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    // ********** SOUND EFFECTS **********
    // ASSIGN SFX TO THE AUDIO MIXER BASED ON THE FOLLOWING GUIDELINES
    // Ambient SFX: Sounds which add complexity to the soundscape but are okay to override (e.g. footsteps, long passive effects)
    // Default SFX: Middle-ground sounds - not too important but shouldn't be overridden as easily (e.g. most enemy sounds)
    // Priority SFX: Sounds which are especially important to hear for the player (e.g. damage taken, player's attack)
    // BIGSFX: Sounds which take the centerstage and override ALL other audio sources for impact (e.g. North railgun, T4 Sacrifice skills)

    [SerializeField] private OneShotSFXTrack jump;
    [SerializeField] private OneShotSFXTrack lightAttack;
    [SerializeField] private OneShotSFXTrack footsteps;
    [SerializeField] private OneShotSFXTrack railgunAttack;

    public enum SFXTrackName {
        JUMP,
        LIGHT_ATTACK,
        FOOTSTEP,
        RAILGUN_ATTACK
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ISFXTrack GetSFXTrack(SFXTrackName trackName) {
        switch (trackName) {
            case SFXTrackName.JUMP:                 return jump;
            case SFXTrackName.LIGHT_ATTACK:         return lightAttack;
            case SFXTrackName.FOOTSTEP:             return footsteps;
            case SFXTrackName.RAILGUN_ATTACK:       return railgunAttack;
            default:                                return null;
        }
    }

    public void PlaySFXTrack(SFXTrackName trackName) {
        GetSFXTrack(trackName).PlayTrack();
    }
}
