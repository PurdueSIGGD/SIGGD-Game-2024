using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

// Attached to music which can be looped
public class MusicTrack : MonoBehaviour, ILoopable, ITrack
{
    // The two tracks used for smooth looping
    [SerializeField] protected AudioSource[] tracks;

    // The index of the current playing track
    protected int currentTrackOffset = 0;

    // The timestamps of the loop points in seconds
    [SerializeField] protected float loopStart;
    [SerializeField] protected float loopEnd;


    // Keeps track of the couroutine to start/stop the tracks successfully
    protected Coroutine looper;

    // The loudest volume which can be reached by the level tracks
    // The tracks are normalized around this value 
    protected float maxVolume = 1.0f;

    protected bool isPlaying = false;


    void Start() { }
    void Update() { }

    public void PlayTrack() {
        if (isPlaying) { return; }
        isPlaying = true;

        looper = StartCoroutine(AutoLoop());
    }

    public void StopTrack() {
        isPlaying = false;
        StopCoroutine(looper);
    }
 
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
            float trackMajorityLength = (loopEnd - tracks[currentTrackOffset].time) * 0.98f;
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
