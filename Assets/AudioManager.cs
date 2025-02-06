using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private float tempStepCounter = 0.0f;

    // SOUNDTRACK
    [SerializeField] AudioTrack japanTheme;
    
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

    public MusicTrackName currentTrack;
    // Start is called before the first frame update
    void Start() {
        japanTheme.Play();
        currentTrack = MusicTrackName.JAPAN;
    }

    // Update is called once per frame
    void Update() {
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
            tempStepCounter += Time.deltaTime;
            if (tempStepCounter > 0.4f) {
                footstep.Play();
                tempStepCounter = 0.0f;
            }
        }   

        if (Input.GetKeyDown(KeyCode.Space)) {
            jump.Play();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            lightAttack.Play();
        }
    }
}
