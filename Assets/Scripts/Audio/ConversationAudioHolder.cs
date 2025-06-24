using UnityEngine;
using System.Collections.Generic;

public class ConversationAudioHolder : MonoBehaviour {
    [SerializeField] public List<OneShotVATrack> tracks;

    public void PlayTrack(int trackId) {
        tracks[trackId].PlayTrack();
    }

    public void StopTrack(int trackId) {
        tracks[trackId].StopTrack();
    }
}