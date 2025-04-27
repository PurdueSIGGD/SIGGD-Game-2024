using System.Collections.Generic;
using UnityEngine;

public class SoundBankVATrack : AbstractSoundBank, IVATrack {

    public bool OverridesVoiceCulling() {
        OneShotVATrack recentTrack = (OneShotVATrack) GetMostRecentTrack();
        return recentTrack.OverridesVoiceCulling();
    }
}