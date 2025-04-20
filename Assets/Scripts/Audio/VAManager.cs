using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VAManager : MonoBehaviour {

    [SerializeField] private VATrack britishAnt;

    public enum VATrackName {
        BRITISH_ANT // If this is still here, remove it
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public VATrack GetDialogueTrack(VATrackName trackName) {
        switch (trackName) {
            case VATrackName.BRITISH_ANT:     return britishAnt;
            default:                                return null;
        }
    }

    public void PlayDialogueTrack(VATrackName trackName) {
        GetDialogueTrack(trackName).PlayTrack();
    }
}