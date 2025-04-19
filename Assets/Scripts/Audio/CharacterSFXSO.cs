using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterSFX", order = 4)]
public class CharacterSFX : ScriptableObject
{
    [System.Serializable]
    public class SFXPair {
        public AudioManager.SFXTrackName trackName;
        public AudioTrack SFXFile;
    }

    public List<SFXPair> charactersSFXList = new List<SFXPair>();
    private Dictionary<AudioManager.SFXTrackName, AudioTrack> charactersSFXdict = new Dictionary<AudioManager.SFXTrackName, AudioTrack>();

    void Start() {
        foreach (var pair in charactersSFXList) {
            charactersSFXdict[pair.trackName] = pair.SFXFile;
        }
    }
}
