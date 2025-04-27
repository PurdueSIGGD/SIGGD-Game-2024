using UnityEngine;

public class OneShotSFXTrack : MonoBehaviour, ISFXTrack {
    
    [SerializeField] private AudioSource track;

    // The predetermined pitch values which define the working pitch range of the sound effect
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    public void PlayTrack() {
        track.PlayOneShot(track.clip, 1.0f);
    }

    public void SetPitch(float currentValue, float maxValue) {
        track.pitch = Mathf.Lerp(minPitch, maxPitch, currentValue / maxValue);
    }
}