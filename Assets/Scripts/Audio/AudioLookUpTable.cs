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
    public List<VABank> VABanks = new();
    public List<VATrack> VATracks = new();

    public Dictionary<string, ISFXTrack> sfxTable;
    public Dictionary<string, ConversationAudioHolder> conversationTable;
    public Dictionary<string, IVATrack> vaTable;

    void Start()
    {
        sfxTable = new Dictionary<string, ISFXTrack>();
        foreach (SFXBank bank in SFXBanks) sfxTable.Add(bank.name, bank.soundBank);
        foreach (SFXLoop loop in SFXLoops) sfxTable.Add(loop.name, loop.soundHolder);
        foreach (SFXTrack track in SFXTracks) sfxTable.Add(track.name, track.soundHolder);

        conversationTable = new Dictionary<string, ConversationAudioHolder>();
        foreach (ConversationTrack conversation in conversationTracks) conversationTable.Add(conversation.name, conversation.audioHolder);

        vaTable = new Dictionary<string, IVATrack>();
        foreach (VABank bank in VABanks) vaTable.Add(bank.name, bank.soundBankVATrack);
        foreach (VATrack track in VATracks) vaTable.Add(track.name, track.oneShotVATrack);

        foreach (string key in vaTable.Keys)
        {
            Debug.Log(key + ": " + vaTable[key]);
        }
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

[Serializable]
public struct VABank
{
    public string name;
    public SoundBankVATrack soundBankVATrack;

    public VABank(string name, SoundBankVATrack soundBankVATrack)
    {
        this.name = name;
        this.soundBankVATrack = soundBankVATrack;
    }
}

[Serializable]
public struct VATrack
{
    public string name;
    public OneShotVATrack oneShotVATrack;

    public VATrack(string name, OneShotVATrack oneShotVATrack)
    {
        this.name = name;
        this.oneShotVATrack = oneShotVATrack;
    }
}