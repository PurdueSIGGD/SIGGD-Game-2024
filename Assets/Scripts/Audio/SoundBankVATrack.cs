using System.Collections.Generic;
using UnityEngine;

public class SoundBankVATrack : AbstractSoundBank, IVATrack {
    
    [Header("Whether this track will play outside of combat")]
    public bool playsOutsideCombat;

    public bool OverridesVoiceCulling() {
        OneShotVATrack recentTrack = (OneShotVATrack) GetMostRecentTrack();
        return recentTrack.OverridesVoiceCulling();
    }

    public bool PlaysOutsideOfCombat()
    {
        return playsOutsideCombat;
    }
}