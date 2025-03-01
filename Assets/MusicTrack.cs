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
    private int currentTrackId = 0;
    // An array of both tracks
    [SerializeField] AudioSource[] tracks;

    void Start() { 
        // If loopEnd is the same as the end of the track, adjust it
        if (Math.Abs(loopEnd - tracks[0].clip.length) < 0.01f) {
            loopEnd = tracks[0].clip.length;
        }
    }

    // Update is called once per frame
    void Update() { }

    public void PlayTrack() {
        tracks[0].Play();
        currentTrackId = 0;
        StartCoroutine(autoLoop());
    }

    public void StopTrack() {
        tracks[0].Stop();
        tracks[1].Stop();
        StopCoroutine(autoLoop());
    }

    private IEnumerator autoLoop() {
        float trackPlaytime = loopEnd - tracks[currentTrackId].time;
        // Wait most of the track
        yield return new WaitForSecondsRealtime(trackPlaytime - 5.0f);
        // Wait for the rest of the track to avoid weird playing offset
        trackPlaytime = loopEnd - tracks[currentTrackId].time;
        yield return new WaitForSecondsRealtime(trackPlaytime);

        while (true) {
            currentTrackId = 1 - currentTrackId;
            tracks[currentTrackId].Play();
            tracks[currentTrackId].time = loopStart;
            trackPlaytime = loopEnd - tracks[currentTrackId].time;
            yield return new WaitForSecondsRealtime(trackPlaytime - 5.0f);
            trackPlaytime = loopEnd - tracks[currentTrackId].time;
            yield return new WaitForSecondsRealtime(trackPlaytime);
        }
    }
}
