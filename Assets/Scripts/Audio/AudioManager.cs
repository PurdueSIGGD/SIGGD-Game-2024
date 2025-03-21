using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private float tempStepCounter = 0.0f;

    // ********** MUSIC **********
    // Has type MusicTrack with "music_" variable name header
    // Accessed externally via PlayMusictrack()/GetMusicTrack() with MusicTrackName parameter

    [Header("Music")]
    [SerializeField] MusicTrack music_japan;
    [SerializeField] MusicTrack music_seamstress;
    

    // ********** SOUND EFFECTS **********
    // Has type AudioTrack with "SFX_" variable name header 
    // Accessed externally via PlaySFXtrack()/GetSFXTrack() with SFXTrackName parameter
    [Space(10)]
    [Header("SFX")]
    [SerializeField] AudioTrack SFX_jump;
    [SerializeField] AudioTrack SFX_lightAttack;
    [SerializeField] AudioTrack SFX_footsteps;


    // ********** DIALOGUE **********
    // Has type AudioTrack with "dialogue_" variable name header
    // Accessed externally via PlayDialoguetrack()/GetDialogueTrack() with DialogueTrackName parameter
    [Space(10)]
    [Header("Dialogue")]
    [SerializeField] AudioTrack dialogue_britishAnt;


    private MusicTrackName currentTrackName;

    // Energy level: This variable is set by other scripts to manage the tracks energy levels
    // 0.0 to 1.0, low energy to high energy
    // Track 1 plays when energy is between 0.0 and 0.5 with greatest volume at energy = 0.0
    // Track 2 always plays and it's volume is greatest when energy = 0.5
    // Track 3 plays when energy is between 0.5 and 1.0 with greatest volume at energy = 1.0
    [Space(10)]
    [SerializeField] float energyLevel;

    // Start is called before the first frame update
    void Start() {
        currentTrackName = MusicTrackName.JAPAN;
        GetCurrentMusicTrack().PlayTrack();
    }

    // Update is called once per frame
    void Update() {
        // Temporary audio test code: remove lines below when done
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
            tempStepCounter += Time.deltaTime;
            if (tempStepCounter > 0.4f) {
                SFX_footsteps.PlayTrack();
                tempStepCounter = 0.0f;
            }
        }   

        if (Input.GetKeyDown(KeyCode.Space)) {
            SFX_jump.PlayTrack();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            SFX_lightAttack.PlayTrack();
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            dialogue_britishAnt.PlayTrack();
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            MusicTrackName nextTrack = currentTrackName == MusicTrackName.JAPAN ? MusicTrackName.SEAMSTRESS : MusicTrackName.JAPAN;
            StartCoroutine(CrossfadeTo(nextTrack, 3.0f));
        }

        if (Input.GetKeyDown(KeyCode.O)) {
            GetCurrentMusicTrack().StopTrack();
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            GetCurrentMusicTrack().PlayTrack();
        }
    }

    // ********** ********** ********** ********** ********** //
    // ******************** Music Interface ******************** //    
    // ********** ********** ********** ********** ********** //

    public enum MusicTrackName {
        //                  loopStart       loopEnd
        JAPAN, //           21.943          197.486
        SEAMSTRESS //       11.912          83.383    
    }

    // Fades into the given track over fadeTime seconds
    public IEnumerator CrossfadeTo(MusicTrackName trackName, float fadeTime) {
        if (fadeTime <= 0) {
            PlayMusicTrack(trackName);
            yield return null;
        }

        int fadeSteps = 20;
        float stepTime = fadeTime / fadeSteps;
        MusicTrack originalTrack = GetMusicTrack(currentTrackName);
        MusicTrack newTrack = GetMusicTrack(trackName);
        
        // Need to be careful since these tracks might already be playing and have their own volume
        float originalTrackStartVolume = originalTrack.GetTrackVolume();
        float newTrackStartVolume = newTrack.GetTrackVolume();
        Debug.Log("Crossfading");

        // The rate to change the tracks' volumes
        float originalTrackVolumeDelta = -originalTrackStartVolume / fadeSteps;
        float newTrackVolumeDelta = (1 - newTrackStartVolume) / fadeSteps;
        Debug.Log("Do: " + originalTrackVolumeDelta + "\tDn" + newTrackVolumeDelta);

        // Fade by adjusting volume over multiple steps
        newTrack.PlayTrack();  
        for (int i = 0; i <= fadeSteps; i++) {
            float originalTrackVolumeAdjustment = originalTrackVolumeDelta * i + originalTrackStartVolume;
            float newTrackVolumeAdjustment = newTrackVolumeDelta * i + newTrackStartVolume;


            originalTrack.SetTrackVolume(originalTrackVolumeAdjustment);
            newTrack.SetTrackVolume(newTrackVolumeAdjustment);            
            yield return new WaitForSeconds(stepTime);
        }
        originalTrack.StopTrack();
        currentTrackName = trackName;
    }

    // Swaps the current track with NO crossfade
    public void PlayMusicTrack(MusicTrackName trackName) {
        GetCurrentMusicTrack().StopTrack();
        currentTrackName = trackName;
        GetCurrentMusicTrack().PlayTrack();
    }

    public MusicTrack GetCurrentMusicTrack() {
        return GetMusicTrack(currentTrackName);
    }

    public MusicTrack GetMusicTrack(MusicTrackName trackName) {
        switch (trackName) {
            case MusicTrackName.JAPAN:              return music_japan;
            case MusicTrackName.SEAMSTRESS:         return music_seamstress;
            default:                                return null;
        }
    }

    // ********** ********** ********** ********** ********** //
    // ******************** SFX Interface ******************** //
    // ********** ********** ********** ********** ********** //

    public enum SFXTrackName {
        JUMP,
        LIGHT_ATTACK,
        FOOTSTEP
    }

    public void PlaySFXTrack(SFXTrackName trackName) {
        GetSFXTrack(trackName).PlayTrack();
    }

    public AudioTrack GetSFXTrack(SFXTrackName trackName) {
        switch (trackName) {
            case SFXTrackName.JUMP:                 return SFX_jump;
            case SFXTrackName.LIGHT_ATTACK:         return SFX_lightAttack;
            case SFXTrackName.FOOTSTEP:             return SFX_footsteps;
            default:                                return null;
        }
    }

    // ********** ********** ********** ********** ********** //
    // ******************** Dialogue Interface ******************** //
    // ********** ********** ********** ********** ********** //

    public enum DialogueTrackName {
        BRITISH_ANT // If this is still here, remove it
    }

    public void PlayDialogueTrack(DialogueTrackName trackName) {
        GetDialogueTrack(trackName).PlayTrack();
    }

    public AudioTrack GetDialogueTrack(DialogueTrackName trackName) {
        switch (trackName) {
            case DialogueTrackName.BRITISH_ANT:     return dialogue_britishAnt;
            default:                                return null;
        }
    }

    // ********** ********** ********** ********** ********** //
    // ******************** Misc Functions ******************** //
    // ********** ********** ********** ********** ********** //
    
    // Used by MusicTracks to find out which track energy levels to play
    public float GetEnergyLevel() { return energyLevel; }

    // Used by tracks outside of audio to set the ~mood~
    public void SetEnergyLevel(float newLevel) { energyLevel = newLevel; }
}
