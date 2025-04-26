using UnityEngine;

public class OneShotVATrack : MonoBehaviour, IVATrack {
    
    [SerializeField] private AudioSource track;

    public void PlayTrack() {
        track.PlayOneShot(track.clip, 1.0f);
    }
}