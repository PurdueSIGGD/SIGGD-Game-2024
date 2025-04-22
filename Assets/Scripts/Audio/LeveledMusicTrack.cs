
using System;
using System.Collections;
using UnityEngine;

// Introduces varying intensity levels found in level themes (and possible boss fight music)
public class LeveledMusicTrack : MusicTrack {

    // Tracks array must have 6 tracks
    // index 0: Level 1, copy 1
    // index 1: Level 1, copy 2
    // index 2: Level 2, copy 1
    // index 3: Level 2, copy 2
    // index 4: Level 3, copy 1
    // index 5: Level 3, copy 2
    private const int TRACK_COUNT = 6;
    [SerializeField] private AudioSource[] tracksLMT = new AudioSource[TRACK_COUNT];

    // Offset shifts between the levels (via the variables below)
    // When 0, plays tracks 0, 2, and 4
    // When 1, plays tracks 1, 3, and 5

    private const int LEVEL_ONE_TRACK_OFFSET = 0;   // Shifts to level 1 tracks
    private const int LEVEL_TWO_TRACK_OFFSET = 2;   // Shifts to level 2 tracks
    private const int LEVEL_THREE_TRACK_OFFSET = 4; // Shifts to level 3 tracks

    // Used to manage which tracks to play
    private bool isHighEnergy = false;
    
    void Start() {
        tracksLMT = new AudioSource[TRACK_COUNT];
        // If loopEnd is the same as the end of the track, adjust it
        if (tracksLMT.Length != TRACK_COUNT) {
            Debug.Log("Hi there! You don't have exactly " + TRACK_COUNT + " tracks in your looping sound! Something's going to break :)");
        }
        if (Math.Abs(loopEnd - tracksLMT[0].clip.length) < 0.05f) {
            loopEnd = tracksLMT[0].clip.length;
        }

        // Set volumes to 0
        foreach (var track in tracksLMT) {
            track.volume = 0.0f;
        }
        //StartCoroutine(Debug_Track_Status());
    }

    void Update() { 
        if (isPlaying) {
            // Tests if the energy level went from above to below high energy or vice versa
            // If so, we need to flip between tracks 1 and 3
            AudioManager am = GetComponentInParent<AudioManager>();
            float energyLevel = am.GetEnergyLevel();
            bool didEnergySwap = energyLevel < 0.5 == isHighEnergy;
            isHighEnergy = energyLevel > 0.5;
            // Keep the music level on par with the player's experience
            // This will probably be a variable external to the audio system later
            // But for now (and testing), it's in this class
            if (didEnergySwap) {
                // Flipped from low to high energy
                if (isHighEnergy) {
                    tracksLMT[currentTrackOffset + LEVEL_ONE_TRACK_OFFSET].Stop();
                    tracksLMT[currentTrackOffset + LEVEL_THREE_TRACK_OFFSET].Play();
                    tracksLMT[currentTrackOffset + LEVEL_THREE_TRACK_OFFSET].time = tracksLMT[currentTrackOffset + LEVEL_TWO_TRACK_OFFSET].time;

                // Flipped from high to low energy
                } else {
                    tracksLMT[currentTrackOffset + LEVEL_THREE_TRACK_OFFSET].Stop();
                    tracksLMT[currentTrackOffset + LEVEL_ONE_TRACK_OFFSET].Play();
                    tracksLMT[currentTrackOffset + LEVEL_ONE_TRACK_OFFSET].time = tracksLMT[currentTrackOffset + LEVEL_TWO_TRACK_OFFSET].time;
                }
            }
            AdjustLeveledTrackVolumes();
        }
    }

    new public void PlayTrack() {
        if (isPlaying) { return; }

        // Play base level 2 (since it always plays)
        tracksLMT[currentTrackOffset + LEVEL_TWO_TRACK_OFFSET].Play();
        // Play level 1 or 3 based on current energy level
        float energyLevel = AudioManager.Instance.GetEnergyLevel();
        if (energyLevel < 0.5) {
            tracksLMT[currentTrackOffset + LEVEL_ONE_TRACK_OFFSET].Play();
            isHighEnergy = false;
        } else if (energyLevel > 0.5) {
            tracksLMT[currentTrackOffset + LEVEL_THREE_TRACK_OFFSET].Play();
            isHighEnergy = true;
        }
        
        isPlaying = true;
        looper = StartCoroutine(AutoLoop());
    }

    new public void StopTrack() {
        foreach (var track in tracksLMT) {
            track.Stop();
        }

        isPlaying = false;
        StopCoroutine(looper);
    }

    new public void SetTrackVolume(float volume) {
        maxVolume = volume;
        float tempv1 = maxVolume - Math.Abs(AudioManager.Instance.GetEnergyLevel() - (maxVolume / 2)) * 2;
        float tempv2 = maxVolume - tempv1;
        AdjustLeveledTrackVolumes();
    }

    // Sets track volumes to match energy levels
    private void AdjustLeveledTrackVolumes() {
        AudioManager am = GetComponentInParent<AudioManager>();
        // Calculates the volume of the level two track, normalizing around the maxVolume value
        float levelTwoTrackVolume = -2 * maxVolume * Math.Abs(am.GetEnergyLevel() - 0.5f) + maxVolume;
        float levelOneOrThreeVolume = maxVolume - levelTwoTrackVolume;
        if (isHighEnergy) {
            tracksLMT[currentTrackOffset + LEVEL_THREE_TRACK_OFFSET].volume = levelOneOrThreeVolume;
        } else {
            tracksLMT[currentTrackOffset].volume = levelOneOrThreeVolume;
        }
        tracksLMT[currentTrackOffset + LEVEL_TWO_TRACK_OFFSET].volume = levelTwoTrackVolume;
    }

    protected override IEnumerator AutoLoop() {
        float trackPlaytime = loopEnd - tracksLMT[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].time;

        do {
            float trackMajorityLength = (loopEnd - tracksLMT[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].time) * TRACK_MAJORITY_RATIO;
            yield return new WaitForSecondsRealtime(trackMajorityLength);
            trackPlaytime = loopEnd - tracksLMT[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].time;
            yield return new WaitForSecondsRealtime(trackPlaytime);

            currentTrackOffset = 1 - currentTrackOffset;

            // Play the right tracks based on energy level and set playback to loopStart
            tracksLMT[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].Play();
            tracksLMT[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].time = loopStart;
            if (isHighEnergy) {
                tracksLMT[LEVEL_THREE_TRACK_OFFSET + currentTrackOffset].Play();
                tracksLMT[LEVEL_THREE_TRACK_OFFSET + currentTrackOffset].time = loopStart;
            } else {
                tracksLMT[currentTrackOffset].Play();
                tracksLMT[currentTrackOffset].time = loopStart;
            }
        } while (true);
    }

    // Debug tool: Call this to get music information
    private IEnumerator Debug_Track_Status() {
        while (true) {
            String msg = "";
            for (int i = 0; i < tracksLMT.Length; i++) {
                msg += "Track " + i + "\tPlaying? [";
                msg += tracksLMT[i].isPlaying ? "X" : " ";
                msg += "]\tVolume:" + tracksLMT[i].volume;
                msg += "\tTime: " + tracksLMT[i].time + "\n";
            }
            Debug.Log(msg);
            yield return new WaitForSeconds(1.0f);
        }
    }
}