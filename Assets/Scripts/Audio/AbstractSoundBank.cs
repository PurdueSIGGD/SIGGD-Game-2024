using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A different type of SFX or dialogue file with multiple sound variations
[Serializable]
public abstract class AbstractSoundBank : MonoBehaviour, ITrack {

    // We could revert the grouping of the sound bank and go back to having a public sounds and weights without the custom pair class
    // Or remove the track count variable so you don't have to redefine the track variable (size would be set in inspector :( )
    public List<UnityEngine.Object> sounds;
    // The probability of choosing a sound in the bank relative to other sounds
    public List<float> soundWeights;

    // Used to prevent the n most recent sounds from playing again
    public int recencyBlacklistSize;

    // The indices of the most recent sounds in the soundbank
    protected List<int> recentSounds = new List<int>();

    protected List<float> cumulativeWeights;

    void Start() {
        GenerateCumulativeWeights();
    }

    public void PlayTrack() {
        float randomNum = UnityEngine.Random.Range(0.0f, cumulativeWeights[cumulativeWeights.Count - 1]);

        // Find a sound to play
        int soundIndex = 0;
        while (soundIndex < cumulativeWeights.Count - 1 && randomNum > cumulativeWeights[soundIndex]) {
            soundIndex++;
        }
        (sounds[soundIndex] as ITrack)?.PlayTrack();

        // Update recent sounds list
        recentSounds.Add(soundIndex);
        if (recentSounds.Count > recencyBlacklistSize) {
            recentSounds.RemoveAt(0);
        }
        GenerateCumulativeWeights();
    }

    // Here's the gist: Don't increase the cumulative weight on blacklisted tracks and they'll never be selected 
    private void GenerateCumulativeWeights() {
        float cumulativeWeight = 0.0f;
        cumulativeWeights = new List<float>();
        for (int i = 0; i < soundWeights.Count; i++) {
            // Only account for sounds which can play
            if (!recentSounds.Contains(i)) {
                cumulativeWeight += soundWeights[i];
            }
            cumulativeWeights.Add(cumulativeWeight);    
        }
    }

    public IVATrack GetMostRecentTrack() {
        return (IVATrack) sounds[recentSounds.Count - 1];
    }
}