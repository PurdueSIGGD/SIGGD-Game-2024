using System;
using System.Collections;
using UnityEngine;

public class LoopingSFXTrack : AbstractLoopingTrack, ISFXTrack {

    // The predetermined pitch values which define the working pitch range of the sound effect
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    // Pitch modifies length, so the true length is kept track of here
    private float pitchAdjustedTrackLength;

    void Start() {
        if (minPitch == 0 || maxPitch == 0) {
            Debug.Log("Woah there! One of your pitch values is 0, that'll break things!" + gameObject);
        }
    }

    public void setPitch(float currentValue, float maxValue) {
        float pitch = currentValue / maxValue;
        foreach (var track in tracks) {
            track.pitch = Mathf.Lerp(minPitch, maxPitch, pitch);
        }
        pitchAdjustedTrackLength = tracks[0].clip.length / Math.Abs(tracks[0].pitch);
    }

    // Tricky since the duration of tracks changes based on pitch and changes happen any time
    // Doesn't work perfectly but only seems to break at extreme pitches over 5-10+ seconds on short SFX
    protected override IEnumerator AutoLoop() {
        do {
            float trackPlaytime = loopEnd - tracks[currentTrackOffset].time;
            float loopPointEpsilon = 0.01f;
            // Don't WaitForseconds if below the threshold
            float noWaitThreshold = 0.05f; // Seconds
            // Carefully wait as to not miss the end of the track
            while (trackPlaytime > noWaitThreshold) {
                Debug.Log(trackPlaytime);
                // Since pitch changes length, we can only wait up to 1/maxPitch of the remaining track length at a time
                // In practice this value is even smaller so we're using a safe pitch value
                float safePitch = maxPitch * 1.2f;
                float maxPitchTime = trackPlaytime / safePitch * TRACK_MAJORITY_RATIO;
                yield return new WaitForSecondsRealtime(maxPitchTime);
                trackPlaytime = loopEnd - tracks[currentTrackOffset].time;
            }

            // Now wait "furiously"
            // This loop may affect performance but hopefully not
            while (trackPlaytime > loopPointEpsilon) {
                trackPlaytime = loopEnd - tracks[currentTrackOffset].time;
            }

            currentTrackOffset = 1 - currentTrackOffset;

            // Play the right tracks based on energy level and set playback to loopStart
            tracks[currentTrackOffset].Play(); 
            tracks[currentTrackOffset].time = loopStart;
        } while (true);
    } 
}