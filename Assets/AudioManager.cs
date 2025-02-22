using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private float tempStepCounter = 0.0f;

    // SOUNDTRACK
    [SerializeField] MusicTrack japanTheme;
    
    public enum MusicTrackName {
        JAPAN
    }

    // SFX
    [SerializeField] AudioTrack jump;
    [SerializeField] AudioTrack lightAttack;
    [SerializeField] AudioTrack footstep;
    public enum SFXTrackName {
        JUMP,
        LIGHT_ATTACK,
        FOOTSTEP
    }

    private MusicTrackName currentTrackName;
    // Start is called before the first frame update

    void Start() {
        currentTrackName = MusicTrackName.JAPAN;
        getCurrentTrack(currentTrackName).PlayTrack();
    }

    // Update is called once per frame
    void Update() {
        // Temporary audio code below
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
            tempStepCounter += Time.deltaTime;
            if (tempStepCounter > 0.4f) {
                footstep.PlayTrack();
                tempStepCounter = 0.0f;
            }
        }   

        if (Input.GetKeyDown(KeyCode.Space)) {
            jump.PlayTrack();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            lightAttack.PlayTrack();
        }
    }

    // Swaps the current track with no crossfade
    public void changeCurrentTrack(MusicTrackName newTrack) {
        getCurrentTrack(currentTrackName).StopTrack();
        currentTrackName = newTrack;
        getCurrentTrack(currentTrackName).PlayTrack();
    }

    private MusicTrack getCurrentTrack(MusicTrackName trackName) {
        switch (trackName) {
            case MusicTrackName.JAPAN:
                return japanTheme;
            default:
                return null;
        }
    }
}
