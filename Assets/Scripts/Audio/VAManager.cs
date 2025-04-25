using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VAManager : MonoBehaviour {
    // Ability lines, etc.
    [SerializeField] private SoundBankVATrack evaExersion;

    // Converstaions
    [SerializeField] private ConversationAudioHolder eva2Orion1;

    // Start is called before the first frame update    
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public IVATrack GetVATrack(VATrackName trackName) {
        switch(trackName) {
            case VATrackName.EVA_EXERSION:    return evaExersion;
            default:                                return null;
        }
    }

    public void PlayVATrack(VATrackName trackName) {
        GetVATrack(trackName).PlayTrack();
    }

    public ConversationAudioHolder GetConversation(ConversationName conversationName) {
        switch(conversationName) {
            case ConversationName.EVA_ORION_1:      return eva2Orion1; // Test name
            default:                                return null;  
        }
    }

    public void PlayConversationLine(ConversationName convName, int lineNumber) {
        GetConversation(convName).PlayTrack(lineNumber);
    }
}

public enum VATrackName {
    EVA_EXERSION
}

public enum ConversationName {
    EVA_ORION_1
}