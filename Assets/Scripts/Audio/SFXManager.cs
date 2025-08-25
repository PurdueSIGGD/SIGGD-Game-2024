using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    // ********** SOUND EFFECTS **********
    // ASSIGN SFX TO THE AUDIO MIXER BASED ON THE FOLLOWING GUIDELINES
    // Ambient SFX: Sounds which add complexity to the soundscape but are okay to override (e.g. footsteps, long passive effects)
    // Default SFX: Middle-ground sounds - not too important but shouldn't be overridden as easily (e.g. most enemy sounds)
    // Priority SFX: Sounds which are especially important to hear for the player (e.g. damage taken, player's attack)
    // BIGSFX: Sounds which take the centerstage and override ALL other audio sources for impact (e.g. North railgun, T4 Sacrifice skills)

    [SerializeField] private AudioLookUpTable lookUpTable;

    public ISFXTrack GetSFXTrack(string trackName)
    {
        return lookUpTable.sfxTable[trackName];
    }

    public void PlaySFXTrack(string trackName) {
        if (!lookUpTable.sfxTable.ContainsKey(trackName))
        {
            Debug.LogError("Cannot find VA track recorded under name: " + trackName);
            return;
        }
        lookUpTable.sfxTable[trackName].PlayTrack();
    }

    // Looping tracks only!! OneShot and sound banks cannot be stopped
    public void StopSFXTrack(string trackName) {
        ISFXTrack track = lookUpTable.sfxTable[trackName];
        if (track is AbstractLoopingTrack) {
            AbstractLoopingTrack absTrack = (AbstractLoopingTrack) track;
            absTrack.StopTrack();
        }
    }
}
