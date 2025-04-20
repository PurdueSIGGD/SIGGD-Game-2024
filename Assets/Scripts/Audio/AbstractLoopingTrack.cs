using System.Collections;
using UnityEngine;

public abstract class AbstractLoopingTrack : MonoBehaviour {

    // The two tracks used for smooth looping
    [SerializeField] protected AudioSource[] tracks;

    // The index of the current playing track
    protected int currentTrackOffset = 0;

    // The timestamps of the loop points in seconds
    [SerializeField] protected float loopStart;
    [SerializeField] protected float loopEnd;


    // Keeps track of the couroutine to start/stop the tracks successfully
    protected Coroutine looper;

    protected bool isPlaying = false;

    // I noticed that waiting for the duration of whole tracks can get off, so waiting in parts is better
    // By waiting for TRACK_MAJORITY_RATIO of the track, we avoid this issue
    protected const float TRACK_MAJORITY_RATIO = 0.98f;

    public void PlayTrack() {
        if (isPlaying) { return; }
        isPlaying = true;
        tracks[0].Play();
        looper = StartCoroutine(AutoLoop());
    }

    public void StopTrack() {
        isPlaying = false;
        StopCoroutine(looper);
    }

    // Every class needs to define a way to auto loop
    protected abstract IEnumerator AutoLoop();
}