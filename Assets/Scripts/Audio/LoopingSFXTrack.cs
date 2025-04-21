using System.Collections;
using UnityEngine;

public class LoopingSFXTrack : AbstractLoopingTrack, ISFXTrack {

    // The predetermined pitch values which define the working pitch range of the sound effect
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    public void setPitch(float currentValue, float maxValue) {
        // TODO!!! - Don't just copy the pitching - we have multiple tracks which each need their pitches changed
    }

    protected override IEnumerator AutoLoop() {
        float trackPlaytime = loopEnd - tracks[currentTrackOffset].time;

        do {
            float trackMajorityLength = (loopEnd - tracks[currentTrackOffset].time) * TRACK_MAJORITY_RATIO;
            yield return new WaitForSecondsRealtime(trackMajorityLength);
            trackPlaytime = loopEnd - tracks[currentTrackOffset].time;
            yield return new WaitForSecondsRealtime(trackPlaytime);

            currentTrackOffset = 1 - currentTrackOffset;

            // Play the right tracks based on energy level and set playback to loopStart
            tracks[currentTrackOffset].Play();
            tracks[currentTrackOffset].time = loopStart;
        } while (true);
    }    
}