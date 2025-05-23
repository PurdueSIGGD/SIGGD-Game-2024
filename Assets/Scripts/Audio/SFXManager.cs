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

    [SerializeField] private SoundBankSFXTrack footstep;
    [SerializeField] private SoundBankSFXTrack dash;
    [SerializeField] private SoundBankSFXTrack airAttack;
    [SerializeField] private SoundBankSFXTrack ghostSwap;
    [SerializeField] private SoundBankSFXTrack heavyAttack;
    [SerializeField] private SoundBankSFXTrack jump;
    [SerializeField] private SoundBankSFXTrack landing;
    [SerializeField] private SoundBankSFXTrack lightAttack;
    [SerializeField] private LoopingSFXTrack fastFall;
    [SerializeField] private LoopingSFXTrack glide;
    [SerializeField] private OneShotSFXTrack heavyAttackPrimed;
    [SerializeField] private OneShotSFXTrack heavyAttackWindUp;
    //[SerializeField] private LoopingSFXTrack railgunCharge;
    //[SerializeField] private OneShotSFXTrack railgunAttack;

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
            case SFXTrackName.FOOTSTEP:             return footstep;
            case SFXTrackName.DASH: return dash;
            case SFXTrackName.AIR_ATTACK: return airAttack;
            case SFXTrackName.GHOST_SWAP: return ghostSwap;
            case SFXTrackName.HEAVY_ATTACK: return heavyAttack;
            case SFXTrackName.JUMP: return jump;
            case SFXTrackName.LANDING: return landing;
            case SFXTrackName.LIGHT_ATTACK: return lightAttack;
            case SFXTrackName.FAST_FALL: return fastFall;
            case SFXTrackName.GLIDE: return glide;
            case SFXTrackName.HEAVY_ATTACK_PRIMED: return heavyAttackPrimed;
            case SFXTrackName.HEAVY_ATTACK_WIND_UP: return heavyAttackWindUp;
            //case SFXTrackName.RAILGUN_CHARGE:       return railgunCharge;
            //case SFXTrackName.RAILGUN_ATTACK:       return railgunAttack;
            default:                                return null;
        }
    }

    public void PlaySFXTrack(SFXTrackName trackName) {
        
        GetSFXTrack(trackName).PlayTrack();
    }

    // Looping tracks only!! OneShot and sound banks cannot be stopped
    public void StopSFXTrack(SFXTrackName trackName) {
        ISFXTrack track = GetSFXTrack(trackName);
        if (track is AbstractLoopingTrack) {
            AbstractLoopingTrack absTrack = (AbstractLoopingTrack) track;
            absTrack.StopTrack();
        }
    }
}

public enum SFXTrackName {
    FOOTSTEP,
    RAILGUN_CHARGE,
    RAILGUN_ATTACK,
    DASH,
    AIR_ATTACK,
    GHOST_SWAP,
    HEAVY_ATTACK,
    JUMP,
    LANDING,
    LIGHT_ATTACK,
    FAST_FALL,
    GLIDE,
    HEAVY_ATTACK_PRIMED,
    HEAVY_ATTACK_WIND_UP
}
