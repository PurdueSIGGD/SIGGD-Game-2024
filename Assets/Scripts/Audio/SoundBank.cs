using System.Collections.Generic;

// A different type of SFX or dialogue file with multiple sound variations
public class SoundBank<T> : ITrack {

    // Used to prevent the n most recent sounds from playing again
    private int recencyBlacklist;

    // A subset of the sound bank - an ordered list of the most recent sounds
    /* public? serializefield? etc */ List<T> recentVoicelines = new List<T>();
    /* public? serializefield? etc */ List<T> soundBank = new List<T>();
    // The probability of choosing a sound in the bank relative to other sounds
    /* public? serializefield? etc */ List<float> soundWeights = new List<float>();

}