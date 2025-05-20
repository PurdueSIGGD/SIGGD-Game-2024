using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioLookUpTable : MonoBehaviour
{
    public List<Conversation> conversationList = new();

    public Dictionary<string, ConversationAudioHolder> conversationTable;

    void Start()
    {
        conversationTable = new Dictionary<string, ConversationAudioHolder>();
        foreach (Conversation conversation in conversationList)
        {
            conversationTable.Add(conversation.name, conversation.audioHolder);
        }
    }
}

[Serializable]
public struct Conversation
{
    public string name;
    public ConversationAudioHolder audioHolder;

    public Conversation(string name, ConversationAudioHolder audioHolder)
    {
        this.name = name;
        this.audioHolder = audioHolder;
    }
}