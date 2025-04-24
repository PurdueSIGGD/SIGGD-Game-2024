using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

// Attached to music which can be looped
public class MusicTrack : AbstractLoopingTrack, IMusicTrack
{

    private const int TRACK_COUNT = 2;
    //[SerializeField] private AudioSource[] tracksMT = new AudioSource[TRACK_COUNT];
    

    // The loudest volume which can be reached by the level tracks
    // The tracks are normalized around this value 
    protected float maxVolume = 1.0f;

    public void SetTrackVolume(float volume) {
        maxVolume = volume;
        tracks[0].volume = maxVolume;
        tracks[1].volume = maxVolume;
    }

    public float GetTrackVolume() {
        if (!isPlaying) {
            return 0.0f;
        }
        return maxVolume;
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
