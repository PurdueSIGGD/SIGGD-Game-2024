using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }

    // The AudioManager toolbox
    public SFXManager SFXBranch { get; private set; }       // Sound effects such as ghost abilities, enemy sounds, UI sounds, etc.
    public MusicManager MusicBranch { get; private set; }   // The game's soundtrack such as level tracks, boss music, ghost themes, etc.
    public VAManager VABranch { get; private set; }         // Any recorded voice such as conversations, ability voicelines, etc.

    // Variables used for testing audio features
    private float tempStepCounter = 0.0f;
    private float tempPitch = 0.0f;
    private bool tempCharging = false;

    // Energy level: This variable is set by other scripts to manage the tracks energy levels
    // 0.0 to 1.0, low energy to high energy
    // Track 1 plays when energy is between 0.0 and 0.5 with greatest volume at energy = 0.0
    // Track 2 always plays and it's volume is greatest when energy = 0.5
    // Track 3 plays when energy is between 0.5 and 1.0 with greatest volume at energy = 1.0
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
        VABranch = GetComponentInChildren<VAManager>();
    }

    // Start is called before the first frame update
    void Start() {
        //Instance.MusicBranch.PlayMusicTrack(MusicTrackName.JAPAN);
        //Instance.MusicBranch.PlayMusicTrack(MusicTrackName.CYBERPUNK);
    }

    // Update is called once per frames
    void Update() {
        // ========= COMMENT THIS OUT BEFORE BUILDING ==============
        //TestAudioFunctions(); // <----------
    }
    
    // Used by MusicTracks to find out which track energy levels to play
    public float GetEnergyLevel() { return energyLevel; }

    // Used by tracks outside of audio to set the ~mood~
    public void SetEnergyLevel(float newLevel) { energyLevel = newLevel; }

    private void TestAudioFunctions() {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
            tempStepCounter += Time.deltaTime;
            if (tempStepCounter > 0.4f) {
                Instance.SFXBranch.PlaySFXTrack(SFXTrackName.FOOTSTEP);
                tempStepCounter = 0.0f;
            }
        }   

        if (Input.GetKeyDown(KeyCode.B)) {
            Instance.VABranch.PlayVATrack(VATrackName.EVA_EXERSION);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            Instance.SFXBranch.PlaySFXTrack(SFXTrackName.RAILGUN_ATTACK);
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            if (!tempCharging) {
                Instance.SFXBranch.PlaySFXTrack(SFXTrackName.RAILGUN_CHARGE);
            } else {
                Instance.SFXBranch.StopSFXTrack(SFXTrackName.RAILGUN_CHARGE);
            }
            tempCharging = !tempCharging;
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            Instance.VABranch.PlayVATrack(VATrackName.BRITISH_ANT);
        }

        /*
        if (Input.GetKeyDown(KeyCode.C)) {
            MusicTrackName nextTrack = Instance.MusicBranch.GetCurrentMusicTrackName() == MusicTrackName.JAPAN ? MusicTrackName.SEAMSTRESS : MusicTrackName.JAPAN;
            Instance.MusicBranch.CrossfadeTo(nextTrack, 3.0f);
        }
        */

        if (Input.GetKeyDown(KeyCode.O)) {
            Instance.MusicBranch.GetCurrentMusicTrack().StopTrack();
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            Instance.MusicBranch.GetCurrentMusicTrack().PlayTrack();
        }
        if (Input.GetKeyDown(KeyCode.N)) {
            tempPitch = Math.Max(0, tempPitch - 1);
            Instance.SFXBranch.GetSFXTrack(SFXTrackName.RAILGUN_CHARGE).SetPitch(tempPitch, 10);          
        }
        if (Input.GetKeyDown(KeyCode.M)) {
            tempPitch = Math.Min(10, tempPitch + 1);
            Instance.SFXBranch.GetSFXTrack(SFXTrackName.RAILGUN_CHARGE).SetPitch(tempPitch, 10);
        }
    }
}

