using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VAManager : MonoBehaviour {
    // Ability lines, etc.
    [SerializeField] private SoundBankVATrack evaExersion;
    //[SerializeField] private OneShotVATrack britishAnt;

    // Converstaions
    [SerializeField] private ConversationAudioHolder eva2Orion1;

    // Used to avoid voiceline spam
    private float globalVoicelineChance = 1.0f;
    private float voicelineCullingTimer = 0.0f;

    // How much of the global voice line chance recovers per second
    private const float VOICE_LINE_CHANCE_GAIN_PER_SECOND = 1.0f;

    // Start is called before the first frame update    
    void Start() {
        StartCoroutine(Debug_Culling_Status());
    }

    // Update is called once per frame
    void Update() {
        voicelineCullingTimer = Math.Max(0.0f, voicelineCullingTimer - Time.deltaTime);
        if (voicelineCullingTimer == 0.0f) {
            globalVoicelineChance = Math.Min(1.0f, globalVoicelineChance += VOICE_LINE_CHANCE_GAIN_PER_SECOND * Time.deltaTime);
        }
    }

    public IVATrack GetVATrack(VATrackName trackName) {
        switch(trackName) {
            case VATrackName.EVA_EXERSION:    return evaExersion;
            //case VATrackName.BRITISH_ANT:     return britishAnt;
            default:                                return null;
        }
    }

    public void PlayVATrack(VATrackName trackName) {
        float temp = UnityEngine.Random.Range(0, 1.0f);
        bool willPlayTrack = globalVoicelineChance > temp;
        if (willPlayTrack) {
            IVATrack castedTrack = GetVATrack(trackName);
            castedTrack.PlayTrack();
            if (!castedTrack.OverridesVoiceCulling()) {
                float trackLength = 0.0f;
                if (castedTrack is SoundBankVATrack) {
                    SoundBankVATrack recastedTrack = (SoundBankVATrack) castedTrack;
                    OneShotVATrack reRecastedTrack = (OneShotVATrack) recastedTrack.GetMostRecentTrack();
                    trackLength = reRecastedTrack.GetTrackLength();
                }
                if (castedTrack is OneShotVATrack) {
                    OneShotVATrack recastedTrack = (OneShotVATrack) castedTrack;
                    trackLength = recastedTrack.GetTrackLength();
                }
                voicelineCullingTimer = Math.Max(voicelineCullingTimer, trackLength);
                globalVoicelineChance = 0.0f;
            }
        }
    }

    public ConversationAudioHolder GetConversation(ConversationName conversationName) {
        switch(conversationName) {
            case ConversationName.EVA_ORION_1:      return eva2Orion1; // Test name
            default:                                return null;  
        }
    }

    // Don't test for voiceline spam - conversation lines MUST play
    public void PlayConversationLine(ConversationName convName, int lineNumber) {
        GetConversation(convName).PlayTrack(lineNumber);
    }

    public void StopConversationLine(ConversationName convName, int lineNumber) {
        GetConversation(convName).StopTrack(lineNumber);
    }

    public IEnumerator Debug_Culling_Status() {
        while (true) {
            String msg = "";
            msg += "globalVoicelineChance: " + globalVoicelineChance + "\n";
            msg += "voicelineCullingTimer: " + voicelineCullingTimer;
            Debug.Log(msg);
            yield return new WaitForSeconds(1.0f);
        }
    }
}

public enum VATrackName {
    EVA_EXERSION,
    BRITISH_ANT
}

public enum ConversationName {
    EVA_ORION_1
}
