using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

// A different type of SFX or dialogue file with multiple sound variations
[Serializable]
public abstract class AbstractSoundBank : MonoBehaviour, ITrack {

    // We could revert the grouping of the sound bank and go back to having a public sounds and weights without the custom pair class
    // Or remove the track count variable so you don't have to redefine the track variable (size would be set in inspector :( )
    public List<UnityEngine.Object> sounds;
    // The probability of choosing a sound in the bank relative to other sounds
    public List<float> soundWeights;

    // The follow lists only keep track of the out of combat sounds whereas the above
    // lists keep track of all sounds
    public List<UnityEngine.Object> outOfCombatSounds;
    public List<float> outOfCombatSoundWeights;

    // General probability of a sound playing from 0 to 1
    public float playChance = 1f;
    [NonSerialized] public bool lastSkipped = false;

    // Used to prevent the n most recent sounds from playing again
    public int recencyBlacklistSize;

    // The indices of the most recent sounds in the soundbank
    protected List<int> recentSounds = new List<int>();

    // The out of combat equivalent
    protected List<int> outOfCombatRecentSounds = new List<int>();

    protected List<float> cumulativeWeights;

    protected bool inCombat; // used to keep track if player is in combat

    [Header("Whether this bank will play something different outside of combat")]
    public bool playsOutsideCombat;

    void Start() {
        //GenerateCumulativeWeights();
    }

    public void PlayTrack() {

        // Roll to see if this sound will play
        float playChanceRoll = UnityEngine.Random.Range(0f, 1f);
        if (playChanceRoll > playChance)
        {
            lastSkipped = true;
            return;
        }
        lastSkipped = false;

        inCombat = AudioManager.Instance.GetEnergyLevel() >= 0.5f;

        GenerateCumulativeWeights();

        float randomNum = UnityEngine.Random.Range(0.0f, cumulativeWeights[cumulativeWeights.Count - 1]);

        // Find a sound to play
        int soundIndex = 0;
        while (soundIndex < cumulativeWeights.Count - 1 && randomNum > cumulativeWeights[soundIndex]) {
            soundIndex++;
        }

        if (!inCombat && playsOutsideCombat)
        {
            (outOfCombatSounds[soundIndex] as ITrack)?.PlayTrack();
            // Update recent sounds list
            outOfCombatRecentSounds.Add(soundIndex);
            if (outOfCombatRecentSounds.Count > recencyBlacklistSize)
            {
                outOfCombatRecentSounds.RemoveAt(0);
            }
        }
        else
        {
            (sounds[soundIndex] as ITrack)?.PlayTrack();
            // Update recent sounds list
            recentSounds.Add(soundIndex);
            if (recentSounds.Count > recencyBlacklistSize)
            {
                recentSounds.RemoveAt(0);
            }
        }


    }

    // Here's the gist: Don't increase the cumulative weight on blacklisted tracks and they'll never be selected 
    private void GenerateCumulativeWeights() {
        float cumulativeWeight = 0.0f;
        cumulativeWeights = new List<float>();

        // if in combat, grab the whole list
        if (!inCombat && playsOutsideCombat)
        {
            for (int i = 0; i < outOfCombatSoundWeights.Count; i++)
            {
                // Only account for sounds which can play
                if (!outOfCombatRecentSounds.Contains(i))
                {
                    cumulativeWeight += outOfCombatSoundWeights[i];
                }
                cumulativeWeights.Add(cumulativeWeight);
            }
        }
        else // else only grab the out of combat sounds
        {
            for (int i = 0; i < soundWeights.Count; i++)
            {
                // Only account for sounds which can play
                if (!recentSounds.Contains(i))
                {
                    cumulativeWeight += soundWeights[i];
                }
                cumulativeWeights.Add(cumulativeWeight);
            }
        }
    }

    public IVATrack GetMostRecentTrack() {
        if (!inCombat && playsOutsideCombat && outOfCombatSounds.Count > 0)
        {
            return (IVATrack) outOfCombatSounds[outOfCombatRecentSounds.Count - 1];
        }
        if (recentSounds.Count <= 0) return null;
        return (IVATrack) sounds[recentSounds.Count - 1];
    }

    public bool PlaysOutsideOfCombat()
    {
        return playsOutsideCombat;
    }
}