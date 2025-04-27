using System.Collections.Generic;
using UnityEngine;

public class SoundBankVATrack : AbstractSoundBank, IVATrack {
    [SerializeField] private bool voiceCullingOverride;

    public bool overridesVoiceCulling() {
        return voiceCullingOverride;
    }
}