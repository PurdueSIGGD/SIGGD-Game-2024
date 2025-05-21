using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioLookUpTable : MonoBehaviour
{
    public List<SFXBank> SFXBanks = new();
    public List<SFXLoop> SFXLoops = new();
    public List<SFXTrack> SFXTracks = new();
    public List<ConversationTrack> conversationTracks = new();

    public Dictionary<string, ISFXTrack> sfxTable;
    public Dictionary<string, ConversationAudioHolder> conversationTable;

    void Start()
    {
        sfxTable = new Dictionary<string, ISFXTrack>();
        foreach (SFXBank bank in SFXBanks) sfxTable.Add(bank.name, bank.soundBank);
        foreach (SFXLoop loop in SFXLoops) sfxTable.Add(loop.name, loop.soundHolder);
        foreach (SFXTrack track in SFXTracks) sfxTable.Add(track.name, track.soundHolder);

        Debug.Log(sfxTable.Count);

        conversationTable = new Dictionary<string, ConversationAudioHolder>();
        foreach (ConversationTrack conversation in conversationTracks) conversationTable.Add(conversation.name, conversation.audioHolder);
    }
}

[Serializable]
public struct ConversationTrack
{
    public string name;
    public ConversationAudioHolder audioHolder;

    public ConversationTrack(string name, ConversationAudioHolder audioHolder)
    {
        this.name = name;
        this.audioHolder = audioHolder;
    }
}

[Serializable]
public struct SFXBank
{
    public string name;
    public SoundBankSFXTrack soundBank;

    public SFXBank(string name, SoundBankSFXTrack soundBank)
    {
        this.name = name;
        this.soundBank = soundBank;
    }
}

[Serializable]
public struct SFXLoop
{
    public string name;
    public LoopingSFXTrack soundHolder;

    public SFXLoop(string name, LoopingSFXTrack soundHolder)
    {
        this.name = name;
        this.soundHolder = soundHolder;
    }
}

[Serializable]
public struct SFXTrack
{
    public string name;
    public ISFXTrack soundHolder;

    public SFXTrack(string name, OneShotSFXTrack soundHolder)
    {
        this.name = name;
        this.soundHolder = soundHolder;
    }
}