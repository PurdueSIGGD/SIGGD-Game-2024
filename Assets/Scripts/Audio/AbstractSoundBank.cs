using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A different type of SFX or dialogue file with multiple sound variations
[Serializable]
public abstract class AbstractSoundBank : MonoBehaviour, ITrack {

        // We could revert the grouping of the sound bank and go back to having a public sounds and weights without the custom pair class
        // Or remove the track count variable so you don't have to redefine the track variable (size would be set in inspector :( )
    [SerializeField] protected List<UnityEngine.Object> sounds;
    // The probability of choosing a sound in the bank relative to other sounds
    [SerializeField] protected List<float> soundWeights;

    // Used to prevent the n most recent sounds from playing again
    [SerializeField] protected int recencyBlacklistSize;

    // The indices of the most recent sounds in the soundbank
    protected List<int> recentSounds = new List<int>();

    protected List<float> cumulativeWeights;

    void Start() {
        generateCumulativeWeights();
    }

    public void PlayTrack() {
        float randomNum = UnityEngine.Random.Range(0.0f, cumulativeWeights[cumulativeWeights.Count - 1]);

        // Find a sound to play
        int soundIndex = 0;
        while (randomNum > cumulativeWeights[soundIndex]) {
            soundIndex++;
        }
        Debug.Log("attempt play");
        (sounds[soundIndex] as ITrack)?.PlayTrack();
        Debug.Log("it worked?");

        // Update recent sounds list
        recentSounds.Add(soundIndex);
        if (recentSounds.Count > recencyBlacklistSize) {
            recentSounds.RemoveAt(0);
        }
        generateCumulativeWeights();
    }

    // Here's the gist: Don't increase the cumulative weight on blacklisted tracks and they'll never be selected 
    private void generateCumulativeWeights() {
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
}