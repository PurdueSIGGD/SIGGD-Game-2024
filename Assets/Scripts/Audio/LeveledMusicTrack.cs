
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

    // Offset shifts between the levels (via the variables below)
    // When 0, plays tracks 0, 2, and 4
    // When 1, plays tracks 1, 3, and 5

    private const int LEVEL_ONE_TRACK_OFFSET = 0;   // Shifts to level 1 tracks
    private const int LEVEL_TWO_TRACK_OFFSET = 2;   // Shifts to level 2 tracks
    private const int LEVEL_THREE_TRACK_OFFSET = 4; // Shifts to level 3 tracks

    // Used to manage which tracks to play
    private bool isHighEnergy = false;
    
    void Start() {
        // If loopEnd is the same as the end of the track, adjust it
        if (tracks.Length != TRACK_COUNT) {
            Debug.Log("Hi there! You don't have exactly " + TRACK_COUNT + " tracks in your looping sound! Something's going to break :)" + gameObject);
        }
        if (Math.Abs(loopEnd - tracks[0].clip.length) < 0.05f) {
            loopEnd = tracks[0].clip.length;
        }

        // Set volumes to 0
        foreach (var track in tracks) {
            track.volume = 0.0f;
        }
        //StartCoroutine(Debug_Track_Status());
    }

    void Update() { 
        if (isPlaying) {
            // Tests if the energy level went from above to below high energy or vice versa
            // If so, we need to flip between tracks 1 and 3
            float energyLevel = AudioManager.Instance.GetEnergyLevel();
            bool didEnergySwap = energyLevel < 0.5f == isHighEnergy;
            isHighEnergy = energyLevel > 0.5f;
            // Keep the music level on par with the player's experience
            // This will probably be a variable external to the audio system later
            // But for now (and testing), it's in this class
            if (didEnergySwap) {
                // Flipped from low to high energy
                if (isHighEnergy) {
                    tracks[currentTrackOffset + LEVEL_ONE_TRACK_OFFSET].Stop();
                    tracks[currentTrackOffset + LEVEL_THREE_TRACK_OFFSET].Play();
                    tracks[currentTrackOffset + LEVEL_THREE_TRACK_OFFSET].time = tracks[currentTrackOffset + LEVEL_TWO_TRACK_OFFSET].time;

                // Flipped from high to low energy
                } else {
                    tracks[currentTrackOffset + LEVEL_THREE_TRACK_OFFSET].Stop();
                    tracks[currentTrackOffset + LEVEL_ONE_TRACK_OFFSET].Play();
                    tracks[currentTrackOffset + LEVEL_ONE_TRACK_OFFSET].time = tracks[currentTrackOffset + LEVEL_TWO_TRACK_OFFSET].time;
                }
            }
            AdjustLeveledTrackVolumes();
        }
    }

    override public void PlayTrack() {
        Debug.Log("I ran");
        if (isPlaying) { return; }
        foreach (var track in tracks) {
            track.time = 0.0f;
        }

        // Play base level 2 (since it always plays)
        tracks[currentTrackOffset + LEVEL_TWO_TRACK_OFFSET].Play();
        // Play level 1 or 3 based on current energy level
        float energyLevel = AudioManager.Instance.GetEnergyLevel();
        if (energyLevel < 0.5f) {
            tracks[currentTrackOffset + LEVEL_ONE_TRACK_OFFSET].Play();
            isHighEnergy = false;
        } else if (energyLevel > 0.5f) {
            tracks[currentTrackOffset + LEVEL_THREE_TRACK_OFFSET].Play();
            isHighEnergy = true;
        }
        
        isPlaying = true;
        looper = StartCoroutine(AutoLoop());
    }

    new public void StopTrack() {
        foreach (var track in tracks) {
            track.Stop();
        }
        isPlaying = false;
        StopCoroutine(looper);
    }

    new public void SetTrackVolume(float volume) {
        maxVolume = volume;
        AdjustLeveledTrackVolumes();
    }

    // Sets track volumes to match energy levels
    private void AdjustLeveledTrackVolumes() {
        // Calculates the volume of the level two track, normalizing around the maxVolume value
        float levelTwoTrackVolume = -2 * maxVolume * Math.Abs(AudioManager.Instance.GetEnergyLevel() - 0.5f) + maxVolume;
        float levelOneOrThreeVolume = maxVolume - levelTwoTrackVolume;
        if (isHighEnergy) {
            tracks[currentTrackOffset + LEVEL_THREE_TRACK_OFFSET].volume = levelOneOrThreeVolume;
        } else {
            tracks[currentTrackOffset + LEVEL_ONE_TRACK_OFFSET].volume = levelOneOrThreeVolume;
        }
        tracks[currentTrackOffset + LEVEL_TWO_TRACK_OFFSET].volume = levelTwoTrackVolume;
    }

    protected override IEnumerator AutoLoop() {
        float trackPlaytime = loopEnd - tracks[currentTrackOffset + LEVEL_TWO_TRACK_OFFSET].time;

        do {
            float trackMajorityLength = (loopEnd - tracks[currentTrackOffset + LEVEL_TWO_TRACK_OFFSET].time) * TRACK_MAJORITY_RATIO;
            yield return new WaitForSecondsRealtime(trackMajorityLength);
            trackPlaytime = loopEnd - tracks[currentTrackOffset + LEVEL_TWO_TRACK_OFFSET].time;
            yield return new WaitForSecondsRealtime(trackPlaytime);

            currentTrackOffset = 1 - currentTrackOffset;

            // Play the right tracks based on energy level and set playback to loopStart
            tracks[currentTrackOffset + LEVEL_TWO_TRACK_OFFSET].Play();
            tracks[currentTrackOffset + LEVEL_TWO_TRACK_OFFSET].time = loopStart;
            if (isHighEnergy) {
                tracks[currentTrackOffset + LEVEL_THREE_TRACK_OFFSET].Play();
                tracks[currentTrackOffset + LEVEL_THREE_TRACK_OFFSET].time = loopStart;
            } else {
                tracks[currentTrackOffset + LEVEL_ONE_TRACK_OFFSET].Play();
                tracks[currentTrackOffset + LEVEL_ONE_TRACK_OFFSET].time = loopStart;
            }
        } while (true);
    }

    // Debug tool: Call this to get music information
    private IEnumerator Debug_Track_Status() {
        while (true) {
            String msg = "";
            for (int i = 0; i < tracks.Length; i++) {
                msg += "Track " + i + "\tPlaying? [";
                msg += tracks[i].isPlaying ? "X" : " ";
                msg += "]\tVolume:" + tracks[i].volume;
                msg += "\tTime: " + tracks[i].time + "\n";
            }
            Debug.Log(msg);
            yield return new WaitForSeconds(1.0f);
        }
    }
}