
using System.Collections.Generic;
using UnityEngine;

public class SoundBankSFXTrack : AbstractSoundBank, ISFXTrack {

    // The predetermined pitch values which define the working pitch range of the sound effect
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    
    public void setPitch(float currentValue, float maxValue) {
        foreach (var sound in sounds) {
            (sound as ISFXTrack)?.setPitch(currentValue, maxValue);
        }
    }
}