using System.Collections;
using UnityEngine;

public abstract class AbstractLoopingTrack : MonoBehaviour {
    
    private const int TRACK_COUNT = 2;

    // The tracks used for smooth looping
    // Unfortunately I have to name the track variable differently because serialization is my enemy
    [SerializeField] protected AudioSource[] tracks;

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
        tracks = new AudioSource[TRACK_COUNT];
        if (tracks.Length != TRACK_COUNT) {
            Debug.Log("Hi there! You don't have exactly " + TRACK_COUNT + " tracks in your looping sound! Something's going to break :)");
        }
    }

    public void PlayTrack() {
        if (isPlaying) { return; }
        isPlaying = true;
        tracks[0].Play();
        looper = StartCoroutine(AutoLoop());
    }

    public void StopTrack() {
        isPlaying = false;
        if (looper != null) {
            StopCoroutine(looper);
        }
    }

    // Every class needs to define a way to auto loop
    protected abstract IEnumerator AutoLoop();
}