using System;
using System.Collections;
using UnityEngine;

// Attached to music which can be looped
public class MusicTrack : MonoBehaviour
{
    // The timestamps of the loop points in seconds
    [SerializeField] float loopStart;
    [SerializeField] float loopEnd;
    
    // The index of the next track to play
    // When 0, plays tracks 0, 2, and 4
    // When 1, plays tracks 1, 3, and 5
    private int currentTrackOffset = 0;
    private const int LEVEL_TWO_TRACK_OFFSET = 2;
    private const int LEVEL_THREE_TRACK_OFFSET = 4;

    // An array of all tracks
    // index 0: Level 1, copy 1
    // index 1: Level 1, copy 2
    // index 2: Level 2, copy 1 
    // index 3: Level 2, copy 2
    // index 4: Level 3, copy 1
    // index 5: Level 3, copy 2
    [SerializeField] AudioSource[] tracks;

    // 0.0 to 1.0, low energy to high energy
    // Track 1 plays when energy is between 0.0 and 0.5 with greatest volume at energy = 0.0
    // Track 2 always plays and it's volume is greatest when energy = 0.5
    // Track 3 plays when energy is between 0.5 and 1.0 with greatest volume at energy = 1.0
    [SerializeField] float energyLevel;
    private bool isHighEnergy = false;

    void Start() { 
        // If loopEnd is the same as the end of the track, adjust it
        if (Math.Abs(loopEnd - tracks[0].clip.length) < 0.05f) {
            loopEnd = tracks[0].clip.length;
        }
        StartCoroutine(Debug_Track_Status());
    }

    // Update is called once per frame
    void Update() {
        // Tests if the energy level went from above to below high energy or vice versa
        // If so, we need to flip between tracks 1 and 3
        bool didEnergySwap = energyLevel < 0.5 == isHighEnergy;
        isHighEnergy = energyLevel > 0.5;
        // Keep the music level on par with the player's experience
        // This will probably be a variable external to the audio system later
        // But for now (and testing), it's in this class
        if (didEnergySwap) {
            // Flipped from low to high energy
            if (isHighEnergy) {
                tracks[currentTrackOffset].Stop();
                tracks[LEVEL_THREE_TRACK_OFFSET + currentTrackOffset].Play();
                tracks[LEVEL_THREE_TRACK_OFFSET + currentTrackOffset].time = tracks[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].time;

            // Flipped from high to low energy
            } else {
                tracks[LEVEL_THREE_TRACK_OFFSET + currentTrackOffset].Stop();
                tracks[currentTrackOffset].Play();
                tracks[currentTrackOffset].time = tracks[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].time;
            }
        }
        setTrackVolumes();
    }

    public void PlayTrack() {
        // Play base level 2 (since it always plays)
        tracks[LEVEL_TWO_TRACK_OFFSET].Play();
        // Play level 1 or 3 based on current energy level
        if (energyLevel < 0.5) {
            tracks[currentTrackOffset].Play();
            isHighEnergy = false;
        } else if (energyLevel > 0.5) {
            tracks[LEVEL_THREE_TRACK_OFFSET + currentTrackOffset].Play();
            isHighEnergy = true;
        }
        setTrackVolumes();

        currentTrackOffset = 0;
        StartCoroutine(autoLoop());
    }

    public void StopTrack() {
        for (int i = 0; i < tracks.Length; i++) {
            tracks[i].Stop();
        }
        StopCoroutine(autoLoop());
    }

    // Sets track volumes to match energy levels
    private void setTrackVolumes() {
        float levelTwoTrackVolume = 1.0f - Math.Abs(energyLevel - 0.5f) * 2;
        if (isHighEnergy) {
            tracks[LEVEL_THREE_TRACK_OFFSET + currentTrackOffset].volume = 1.0f - levelTwoTrackVolume;
        } else {
            tracks[currentTrackOffset].volume = 1.0f - levelTwoTrackVolume;
        }
        tracks[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].volume = levelTwoTrackVolume;
    }

    private IEnumerator autoLoop() {
        float trackPlaytime = loopEnd - tracks[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].time;
        // Wait most of the track
        float trackMajorityLength = trackPlaytime * 0.98f;
        yield return new WaitForSecondsRealtime(trackMajorityLength);
        // Wait for the rest of the track to avoid weird playing offset
        trackPlaytime = loopEnd - tracks[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].time;
        yield return new WaitForSecondsRealtime(trackPlaytime);

        while (true) {
            currentTrackOffset = 1 - currentTrackOffset;

            // Play the right tracks based on energy level and set playback to loopStart
            tracks[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].Play();
            tracks[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].time = loopStart;
            if (!isHighEnergy) {
                tracks[currentTrackOffset].Play();
                tracks[currentTrackOffset].time = loopStart;
            } else {
                tracks[LEVEL_THREE_TRACK_OFFSET + currentTrackOffset].Play();
                tracks[LEVEL_THREE_TRACK_OFFSET + currentTrackOffset].time = loopStart;
            }

            trackMajorityLength = (loopEnd - tracks[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].time) * 0.98f;
            yield return new WaitForSecondsRealtime(trackMajorityLength);
            trackPlaytime = loopEnd - tracks[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].time;
            yield return new WaitForSecondsRealtime(trackPlaytime);
        }
    }

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
