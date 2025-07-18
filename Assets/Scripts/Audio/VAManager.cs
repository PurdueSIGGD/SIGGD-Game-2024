using System;
using System.Collections;
using UnityEngine;

public class VAManager : MonoBehaviour {

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioLookUpTable lookUpTable;

    // Used to avoid voiceline spam
    private float globalVoicelineChance = 1.0f;
    private float voicelineCullingTimer = 0.0f;

    // How much of the global voice line chance recovers per second
    private const float VOICE_LINE_CHANCE_GAIN_PER_SECOND = 1.0f;

    // Start is called before the first frame update    
    void Awake() {
        audioManager = AudioManager.Instance;
        StartCoroutine(Debug_Culling_Status());
    }

    // Update is called once per frame
    void Update() {
        voicelineCullingTimer = Math.Max(0.0f, voicelineCullingTimer - Time.deltaTime);
        if (voicelineCullingTimer == 0.0f) {
            globalVoicelineChance = Math.Min(1.0f, globalVoicelineChance += VOICE_LINE_CHANCE_GAIN_PER_SECOND * Time.deltaTime);
        }
    }

    public void PlayVATrack(string trackName) {
        if (!lookUpTable.vaTable.ContainsKey(trackName))
        {
            Debug.LogWarning("Cannot find VA track recorded under name: " + trackName);
            return;
        }

        float temp = UnityEngine.Random.Range(0, 1.0f);
        bool willPlayTrack = globalVoicelineChance > temp;
        if (willPlayTrack) {
            IVATrack castedTrack = lookUpTable.vaTable[trackName];
            castedTrack.PlayTrack();
            if (!castedTrack.OverridesVoiceCulling()) {
                float trackLength = 0.0f;
                if (castedTrack is SoundBankVATrack) {
                    SoundBankVATrack recastedTrack = (SoundBankVATrack) castedTrack;
                    if (recastedTrack.lastSkipped) return;
                    OneShotVATrack reRecastedTrack = (OneShotVATrack) recastedTrack.GetMostRecentTrack();
                    if (reRecastedTrack != null) trackLength = reRecastedTrack.GetTrackLength();
                }
                if (castedTrack is OneShotVATrack) {
                    OneShotVATrack recastedTrack = (OneShotVATrack) castedTrack;
                    if (recastedTrack != null) trackLength = recastedTrack.GetTrackLength();
                }
                voicelineCullingTimer = Math.Max(voicelineCullingTimer, trackLength);
                globalVoicelineChance = 0.0f;
            }
        }
    }

    // Don't test for voiceline spam - conversation lines MUST play
    public void PlayConversationLine(string convName, int lineNumber) {
        lookUpTable.conversationTable[convName].PlayTrack(lineNumber);
    }

    public void StopConversationLine(string convName, int lineNumber) {
        lookUpTable.conversationTable[convName].StopTrack(lineNumber);
    }

    public IEnumerator Debug_Culling_Status() {
        while (true) {
            String msg = "";
            msg += "globalVoicelineChance: " + globalVoicelineChance + "\n";
            msg += "voicelineCullingTimer: " + voicelineCullingTimer;
            //Debug.Log(msg);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
