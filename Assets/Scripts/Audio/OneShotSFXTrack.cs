using UnityEngine;

public class OneShotSFXTrack : ISFXTrack {
    
    [SerializeField] private AudioSource track;

    public void PlayTrack() {
        track.PlayOneShot(track.clip, 1.0f);
    }
}