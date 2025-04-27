using System;
using UnityEngine;

public class OneShotVATrack : MonoBehaviour, IVATrack {
    
    [SerializeField] private AudioSource track;

    // Whether this track can be culled by the VAManager
    [SerializeField] private bool voiceCullingOverride;

    public void PlayTrack() {
        track.time = 0.0f;
        track.PlayOneShot(track.clip, 1.0f);
    }

    public void StopTrack() {
        track.Stop();
    }

    // Accessing the track length might be helpful for programmers
    // Also for global voiceline culling
    public float GetTrackLength() {
        return track.clip.length;
    }
    
    public bool overridesVoiceCulling() {
        return voiceCullingOverride;
    }
}