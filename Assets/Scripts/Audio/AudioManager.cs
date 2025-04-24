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
    public VAManager VABranch { get; private set; }         // Any recorded voice such as converstaions, ability voicelines, etc.

    private float tempStepCounter = 0.0f;

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
        Instance.MusicBranch.PlayMusicTrack(MusicManager.MusicTrackName.JAPAN);
        Instance.MusicBranch.GetCurrentMusicTrack().PlayTrack();
    }

    // Update is called once per frame
    void Update() {
        // Temporary audio test code: remove lines below when done
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
            tempStepCounter += Time.deltaTime;
            if (tempStepCounter > 0.4f) {
                Instance.SFXBranch.PlaySFXTrack(SFXManager.SFXTrackName.FOOTSTEP);
                tempStepCounter = 0.0f;
            }
        }   

        if (Input.GetKeyDown(KeyCode.B)) {
            Instance.VABranch.PlayVATrack(VAManager.VATrackName.EVA_EXERSION);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            Instance.SFXBranch.PlaySFXTrack(SFXManager.SFXTrackName.RAILGUN_ATTACK);
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            MusicManager.MusicTrackName nextTrack = 
                    Instance.MusicBranch.GetCurrentMusicTrackName() == MusicManager.MusicTrackName.JAPAN
                    ? MusicManager.MusicTrackName.SEAMSTRESS
                    : MusicManager.MusicTrackName.JAPAN;
            Instance.MusicBranch.CrossfadeTo(nextTrack, 3.0f);
        }

        if (Input.GetKeyDown(KeyCode.O)) {
            Instance.MusicBranch.GetCurrentMusicTrack().StopTrack();
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            Instance.MusicBranch.GetCurrentMusicTrack().PlayTrack();
        }
    }
    
    // Used by MusicTracks to find out which track energy levels to play
    public float GetEnergyLevel() { return energyLevel; }

    // Used by tracks outside of audio to set the ~mood~
    public void SetEnergyLevel(float newLevel) { energyLevel = newLevel; }
}

