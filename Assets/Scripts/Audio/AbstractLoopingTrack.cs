using System.Collections;
using UnityEngine;

public abstract class AbstractLoopingTrack : MonoBehaviour {


    // The tracks used for smooth looping
    // Unfortunately I have to name the track variable differently because serialization is my enemy
    [SerializeField] public AudioSource[] tracks;

    // The index of the current playing track
    protected int currentTrackOffset = 0;

    // The timestamps of the loop points in seconds
    [SerializeField] protected float loopStart;
    [SerializeField] protected float loopEnd;


    // Keeps track of the couroutine to start/stop the tracks successfully
    protected Coroutine looper = null;

    protected bool isPlaying = false;

    // I noticed that waiting for the duration of whole tracks can get off, so waiting in parts is better
    // By waiting for TRACK_MAJORITY_RATIO of the track, we avoid this issue
    protected const float TRACK_MAJORITY_RATIO = 0.98f;

    void Start() {
        if (loopEnd == 0) {
            loopEnd = tracks[0].clip.length;
        }
    }

    public virtual void PlayTrack() {
        if (isPlaying) { return; }
        tracks[0].Play();
        foreach (var track in tracks) {
            track.time = 0.0f;
        }
        isPlaying = true;
        looper = StartCoroutine(AutoLoop());
    }

    public void StopTrack() {
        foreach (var track in tracks) {
            track.Stop();
        }
        isPlaying = false;
        if (looper != null) {
            StopCoroutine(looper);
        }
    }

    // Every class needs to define a way to auto loop
    protected abstract IEnumerator AutoLoop();
}