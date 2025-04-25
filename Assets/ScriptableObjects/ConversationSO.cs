using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ConversationTemp", order = 1)]
public class ConversationTemp : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        public CharacterSO character;
        public string expression;
        [TextArea (5,3)]
        public string line;

    }

    public List<DialogueLine> dialogueLines = new List<DialogueLine>();

    public ConversationName converstaionName;
}
