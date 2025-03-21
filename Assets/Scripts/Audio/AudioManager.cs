using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private float tempStepCounter = 0.0f;

    // SOUNDTRACK
    [SerializeField] MusicTrack japanTheme;
    [SerializeField] MusicTrack seamstressTheme;
    
    public enum MusicTrackName {
        //                  loopStart       loopEnd
        JAPAN, //           21.943          197.486
        SEAMSTRESS //       11.912          83.383    
    }

    // SFX
    [SerializeField] AudioTrack jumpSFX;
    [SerializeField] AudioTrack lightAttackSFX;
    [SerializeField] AudioTrack footstepSFX;
    public enum SFXTrackName {
        JUMP,
        LIGHT_ATTACK,
        FOOTSTEP
    }

    [SerializeField] AudioTrack britishAnt;

    public enum DialogueTrackName {
        BRITISH_ANT // If this is still here, remove it
    }

    private MusicTrackName currentTrackName;

    // Energy level: This variable is set by other scripts to manage the tracks energy levels
    // 0.0 to 1.0, low energy to high energy
    // Track 1 plays when energy is between 0.0 and 0.5 with greatest volume at energy = 0.0
    // Track 2 always plays and it's volume is greatest when energy = 0.5
    // Track 3 plays when energy is between 0.5 and 1.0 with greatest volume at energy = 1.0
    [SerializeField] float energyLevel;

    // Start is called before the first frame update
    void Start() {
        currentTrackName = MusicTrackName.JAPAN;
        GetCurrentTrack().PlayTrack();
    }

    // Update is called once per frame
    void Update() {
        // Temporary audio code below
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
            tempStepCounter += Time.deltaTime;
            if (tempStepCounter > 0.4f) {
                footstepSFX.PlayTrack();
                tempStepCounter = 0.0f;
            }
        }   

        if (Input.GetKeyDown(KeyCode.Space)) {
            jumpSFX.PlayTrack();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            lightAttackSFX.PlayTrack();
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            britishAnt.PlayTrack();
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            MusicTrackName nextTrack = currentTrackName == MusicTrackName.JAPAN ? MusicTrackName.SEAMSTRESS : MusicTrackName.JAPAN;
            StartCoroutine(Crossfade(nextTrack, 10.0f));
        }
    }

    // Swaps the current track with no crossfade
    public void ChangeCurrentTrack(MusicTrackName newTrack) {
        GetCurrentTrack().StopTrack();
        currentTrackName = newTrack;
        GetCurrentTrack().PlayTrack();
    }

    public MusicTrack GetCurrentTrack() {
        switch (currentTrackName) {
            case MusicTrackName.JAPAN:
                return japanTheme;
            case MusicTrackName.SEAMSTRESS:
                return seamstressTheme;
            default:
                return null;
        }
    }

    public MusicTrack GetMusicTrack(MusicTrackName trackName) {
        switch (trackName) {
            case MusicTrackName.JAPAN:
                return japanTheme;
            case MusicTrackName.SEAMSTRESS:
                return seamstressTheme;
            default:
                return null;
        }
    }

    // Used by MusicTracks to find out which track energy levels to play
    public float GetEnergyLevel() { return energyLevel; }

    // Used by tracks outside of audio to set the ~mood~
    public void SetEnergyLevel(float newLevel) { energyLevel = newLevel; }

    // Fades into the given track over fadeTime seconds
    public IEnumerator Crossfade(MusicTrackName trackName, float fadeTime) {
        if (fadeTime <= 0) {
            ChangeCurrentTrack(trackName);
            yield return null;
        }

        int fadeSteps = 20;
        float stepTime = fadeTime / fadeSteps;
        MusicTrack originalTrack = GetMusicTrack(currentTrackName);
        MusicTrack newTrack = GetMusicTrack(trackName);

        // Fade by adjusting volume over multiple steps
        newTrack.PlayTrack();  
        for (int i = 0; i <= fadeSteps; i++) {
            float newVolume = 1.0f / fadeSteps * i;
            originalTrack.SetTrackVolume(1.0f - newVolume);
            newTrack.SetTrackVolume(newVolume);            
            yield return new WaitForSeconds(stepTime);
        }
        originalTrack.StopTrack();
        currentTrackName = trackName;
    }
}
