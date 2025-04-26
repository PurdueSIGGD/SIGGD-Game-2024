using System;
using UnityEngine;

public class OneShotVATrack : MonoBehaviour, IVATrack {
    
    [SerializeField] private AudioSource track;

    // Whether this track can be culled by the VAManager
    [SerializeField] private bool voiceCullingOverride;

    public void PlayTrack() {
        track.PlayOneShot(track.clip, 1.0f);
    }

    // Accessing the track length might be helpful for programmers
    // Also for global voiceline culling
    public float GetTrackLength() {
        return track.clip.length;
    }
}