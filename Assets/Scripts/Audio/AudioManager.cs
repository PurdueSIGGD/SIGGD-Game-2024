using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }

    // The AudioManager toolbox
    public SFXManager SFXBranch { get; private set; }
    public MusicManager MusicBranch { get; private set; }
    // public DialogueManager DialogueBranch { get; private set; } // TODO: uncomment when other dialoguemanger is figured out

    private float tempStepCounter = 0.0f;


    // ********** DIALOGUE **********
    // Has type AudioTrack with "dialogue_" variable name header
    // Accessed externally via PlayDialoguetrack()/GetDialogueTrack() with DialogueTrackName parameter
    [SerializeField] private DialogueTrack dialogue_britishAnt;


    private MusicTrackName currentTrackName;

    // Energy level: This variable is set by other scripts to manage the tracks energy levels
    // 0.0 to 1.0, low energy to high energy
    // Track 1 plays when energy is between 0.0 and 0.5 with greatest volume at energy = 0.0
    // Track 2 always plays and it's volume is greatest when energy = 0.5
    // Track 3 plays when energy is between 0.5 and 1.0 with greatest volume at energy = 1.0
    [Space(10)]
    [SerializeField] float energyLevel;

    void Awake()
    {
        // Make sure there are no duplicates
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SFXBranch = GetComponentInChildren<SFXManager>();
        MusicBranch = GetComponentInChildren<MusicManager>();
        // DialogueBranch = GetComponentInChildren<DialogueManager>(); // TODO uncomment when dialoguemanager is figured out
    }

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

        if (Input.GetKeyDown(KeyCode.R)) {
            SFX_railgunAttack.PlayTrack();
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

