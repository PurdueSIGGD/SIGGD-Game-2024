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
        if (tracks[nextSource].isPlaying) {
            tracks[nextSource].PlayScheduled(AudioSettings.dspTime + (loopEnd - loopStart));
            nextSource = 1 - nextSource;
        }
    }

    public void PlayTrack() {
        tracks[0].Play();
        nextSource = 1;
    }

    public void StopTrack() {
        tracks[0].Stop();
        tracks[1].Stop();
    }
}
