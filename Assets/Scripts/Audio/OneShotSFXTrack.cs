using UnityEngine;

public class OneShotSFXTrack : MonoBehaviour, ISFXTrack {
    
    [SerializeField] public AudioSource track;

    // The predetermined pitch values which define the working pitch range of the sound effect
    public float minPitch = 1;
    public float maxPitch = 1;

    public void PlayTrack() {
        track.PlayOneShot(track.clip, 1.0f);
    }

    public void SetPitch(float currentValue, float maxValue) {
        track.pitch = Mathf.Lerp(minPitch, maxPitch, currentValue / maxValue);
    }
}