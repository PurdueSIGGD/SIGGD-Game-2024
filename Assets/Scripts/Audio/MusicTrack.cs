using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

// Attached to music which can be looped
// Supports varying intensity levels
public class MusicTrack : MonoBehaviour
{
    // The timestamps of the loop points in seconds
    [SerializeField] float loopStart;
    [SerializeField] float loopEnd;

    // The loudest volume which can be reached by the level tracks
    // The tracks are normalized around this value 
    private float maxVolume = 1.0f;
    
    // The index of the next track to play
    // When 0, plays tracks 0, 2, and 4
    // When 1, plays tracks 1, 3, and 5
    private int currentTrackOffset = 0;
    private const int LEVEL_TWO_TRACK_OFFSET = 2;   // Shifts to level 2 tracks
    private const int LEVEL_THREE_TRACK_OFFSET = 4; // Shifts to level 3 tracks

    // An array of all tracks
    // index 0: Level 1, copy 1
    // index 1: Level 1, copy 2
    // index 2: Level 2, copy 1 
    // index 3: Level 2, copy 2
    // index 4: Level 3, copy 1
    // index 5: Level 3, copy 2
    [SerializeField] AudioSource[] tracks;

    // Used to manage which tracks to play
    private bool isHighEnergy = false;

    private bool isPlaying = false;

    Coroutine looper;

    void Start() { 
        // If loopEnd is the same as the end of the track, adjust it
        if (Math.Abs(loopEnd - tracks[0].clip.length) < 0.05f) {
            loopEnd = tracks[0].clip.length;
        }

        // Set volumes to 0
        for (int i = 0; i < tracks.Length; i++) {
            tracks[i].volume = 0.0f;
        }
        StartCoroutine(Debug_Track_Status());
    }

    // Update is called once per frame 
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
            AdjustLeveledTrackVolumes();
        }
    }

    public void PlayTrack() {
        if (isPlaying) { return; }
        
        AudioManager am = GetComponentInParent<AudioManager>();
        // Play base level 2 (since it always plays)
        tracks[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].Play();
        // Play level 1 or 3 based on current energy level
        float energyLevel = am.GetEnergyLevel();
        if (energyLevel < 0.5) {
            tracks[currentTrackOffset].Play();
            isHighEnergy = false;
        } else if (energyLevel > 0.5) {
            tracks[LEVEL_THREE_TRACK_OFFSET + currentTrackOffset].Play();
            isHighEnergy = true;
        }
        isPlaying = true;
        AdjustLeveledTrackVolumes();

        looper = StartCoroutine(AutoLoop());
    }

    public void StopTrack() {
        for (int i = 0; i < tracks.Length; i++) {
            tracks[i].Stop();
        }
        isPlaying = false;
        StopCoroutine(looper);
    }
 
    public void SetTrackVolume(float volume) {
        maxVolume = volume;
        AudioManager am = GetComponentInParent<AudioManager>();
        float tempv1 = maxVolume - Math.Abs(am.GetEnergyLevel() - (maxVolume / 2)) * 2;
        float tempv2 = maxVolume - tempv1;
        Debug.Log("tempv1&2: " + tempv1 + "\t" + tempv2);
        AdjustLeveledTrackVolumes();
    }

    public float GetTrackVolume() {
        if (!isPlaying) {
            return 0.0f;
        }
        return maxVolume;
    }

    // Sets track volumes to match energy levels
    private void AdjustLeveledTrackVolumes() {
        AudioManager am = GetComponentInParent<AudioManager>();
        // Calculates the volume of the level two track, normalizing around the maxVolume value
        float levelTwoTrackVolume = -2 * maxVolume * Math.Abs(am.GetEnergyLevel() - 0.5f) + maxVolume;
        float levelOneOrThreeVolume = maxVolume - levelTwoTrackVolume;
        if (isHighEnergy) {
            tracks[LEVEL_THREE_TRACK_OFFSET + currentTrackOffset].volume = levelOneOrThreeVolume;
        } else {
            tracks[currentTrackOffset].volume = levelOneOrThreeVolume;
        }
        tracks[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].volume = levelTwoTrackVolume;
    }

    private IEnumerator AutoLoop() {
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
            if (isHighEnergy) {
                tracks[LEVEL_THREE_TRACK_OFFSET + currentTrackOffset].Play();
                tracks[LEVEL_THREE_TRACK_OFFSET + currentTrackOffset].time = loopStart;
            } else {
                tracks[currentTrackOffset].Play();
                tracks[currentTrackOffset].time = loopStart;
            }

            trackMajorityLength = (loopEnd - tracks[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].time) * 0.98f;
            yield return new WaitForSecondsRealtime(trackMajorityLength);
            trackPlaytime = loopEnd - tracks[LEVEL_TWO_TRACK_OFFSET + currentTrackOffset].time;
            yield return new WaitForSecondsRealtime(trackPlaytime);
        }
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
