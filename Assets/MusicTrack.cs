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
    private int nextSource;
    // An array of both tracks
    [SerializeField] AudioSource[] tracks;

    void Start() { 
        // If loopEnd is the same as the end of the track, adjust it
        if (Math.Abs(loopEnd - tracks[0].clip.length) < 0.01f) {
            loopEnd = tracks[0].clip.length;
        }
    }

    // Update is called once per frame
    void Update() {
        int currentSource = 1 - nextSource;
        if (!tracks[currentSource].isPlaying) {
            nextSource = 1 - nextSource;
            tracks[nextSource].Play();
            tracks[nextSource].time = loopStart;
        }
    }

    public void PlayTrack() {
        tracks[0].Play();
        double trackTime = tracks[nextSource].time;
        nextSource = 1;
    }

    public void StopTrack() {
        tracks[0].Stop();
        tracks[1].Stop();
    }
}
