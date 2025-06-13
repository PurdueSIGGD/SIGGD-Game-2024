using System;
using System.Collections;
using UnityEngine;

public class LoopingSFXTrack : AbstractLoopingTrack, ISFXTrack {

    // The predetermined pitch values which define the working pitch range of the sound effect
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    [Header("Whether this track will play outside of combat")]
    public bool playsOutsideCombat;

    // Pitch modifies length, so the true length is kept track of here
    private float pitchAdjustedTrackLength;

    void Start() {
        if (minPitch == 0 || maxPitch == 0) {
            Debug.Log("Woah there! One of your pitch values is 0, that'll break things!" + gameObject);
        }
    }

    public void SetPitch(float currentValue, float maxValue) {
        float pitch = currentValue / maxValue;
        foreach (var track in tracks) {
            track.pitch = Mathf.Lerp(minPitch, maxPitch, pitch);
        }
        pitchAdjustedTrackLength = tracks[0].clip.length / Math.Abs(tracks[0].pitch);
    }

    // Looping is actually handled by unity's looping feature
    protected override IEnumerator AutoLoop() { yield return null; }

    public bool PlaysOutsideOfCombat()
    {
        return playsOutsideCombat;
    }
}