using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to SFX and non-loopable sounds like dialogue
public class DialogueTrack : MonoBehaviour, ITrack
{
    // The sound file this script is attached to
    [SerializeField] AudioSource track;
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void PlayTrack() {
        track.PlayOneShot(track.clip, 1.0f);
    }

    public float GetTrackTime() {
        return track.time;
    }
}
