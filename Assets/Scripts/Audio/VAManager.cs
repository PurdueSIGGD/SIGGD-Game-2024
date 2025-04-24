using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VAManager : MonoBehaviour {
    [SerializeField] private SoundBankVATrack evaExersion;

    public enum VATrackName {
        EVA_EXERSION
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public IVATrack GetVATrack(VATrackName trackName) {
        switch (trackName) {
            case VATrackName.EVA_EXERSION:    return evaExersion;
            default:                                return null;
        }
    }

    public void PlayVATrack(VATrackName trackName) {
        GetVATrack(trackName).PlayTrack();
    }
}