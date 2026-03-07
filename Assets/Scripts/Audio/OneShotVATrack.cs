using System;
using UnityEngine;

public class OneShotVATrack : MonoBehaviour, IVATrack {

    public AudioSource track;

    [Header("Whether this track can be culled by the VAManager")]
    public bool voiceCullingOverride;

    [Header("Whether this track will play outside of combat")]
    public bool playsOutsideCombat;

    [Header("Whether this track will play regardless of other playing VA tracks")]
    public bool alwaysPlays;

    public AudioSource PlayTrack() {
        if (!playsOutsideCombat && AudioManager.Instance.GetEnergyLevel() < 0.5f) return null;
        else
        {
            track.time = 0.0f;
            track.PlayOneShot(track.clip, 1.0f);
        }
        return track;
    }

    public void StopTrack() {
        track.Stop();
    }

    // Accessing the track length might be helpful for programmers
    // Also for global voiceline culling
    public float GetTrackLength() {
        return track.clip.length;
    }
    
    public bool OverridesVoiceCulling() {
        return voiceCullingOverride;
    }

    public bool PlaysOutsideOfCombat()
    {
        return playsOutsideCombat;
    }

    public bool AlwaysPlays()
    {
        return alwaysPlays;
    }
}