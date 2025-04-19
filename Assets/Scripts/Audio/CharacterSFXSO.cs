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
        public SFXManager.SFXTrackName trackName;
        public SFXTrack SFXFile;
    }

    public List<SFXPair> charactersSFXList = new List<SFXPair>();
    private Dictionary<SFXManager.SFXTrackName, SFXTrack> charactersSFXdict = new Dictionary<SFXManager.SFXTrackName, SFXTrack>();

    void Start() {
        foreach (var pair in charactersSFXList) {
            charactersSFXdict[pair.trackName] = pair.SFXFile;
        }
    }
}
