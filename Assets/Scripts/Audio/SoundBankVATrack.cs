using System.Collections.Generic;
using UnityEngine;

public class SoundBankVATrack : AbstractSoundBank<IVATrack>, IVATrack {
    // The predetermined pitch values which define the working pitch range of the sound effect
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    [SerializeField] private List<weightedSoundPair> soundBankSBSFXT = new List<weightedSoundPair>();
}