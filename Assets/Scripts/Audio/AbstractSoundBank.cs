using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A different type of SFX or dialogue file with multiple sound variations
public abstract class AbstractSoundBank : MonoBehaviour {

    // Used to prevent the n most recent sounds from playing again
    [SerializeField] protected int recencyBlacklistSize;

    // The indices of the most recent sounds in the soundbank
    [SerializeField] protected List<int> recentSounds = new List<int>();
    [SerializeField] protected List<ITrack> soundBank = new List<ITrack>();
    // The probability of choosing a sound in the bank relative to other sounds
    [SerializeField] protected List<float> soundWeights = new List<float>();

    private List<float> cumulativeWeights;

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
        soundBank[soundIndex].PlayTrack();

        // Update recent sounds list
        recentSounds.Add(soundIndex);
        if (recentSounds.Count > recencyBlacklistSize) {
            recentSounds.RemoveAt(0);
        }
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