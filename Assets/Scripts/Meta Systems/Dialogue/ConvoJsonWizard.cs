#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class ConvoJsonWizard : ScriptableWizard
{
    [SerializeField] string convoName;
    [SerializeField] string text;

    [MenuItem("SIGGD/Create new Conversation SOs")]
    static void CreateWizard()
    {
        DisplayWizard<ConvoJsonWizard>("Convo Creator", "Create ConvoSO");
    }

    private void OnWizardCreate()
    {
        ConversationJson conversationJson = new()
        {
            convoName = convoName,
            lines = new List<LineJson>()
        };

        String linePattern = @"(?=\[)";
        string[] lines = Regex.Split(text, linePattern);

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            
            Regex convoPattern = new(@"\[([^\]]+)\]\s(.+)");
            Match match = convoPattern.Match(line);

            if (match.Success)
            {
                LineJson lineJson = new()
                {
                    character = match.Groups[1].Value.Substring(0, 1) +
                                match.Groups[1].Value.Substring(1).ToLower(),
                    line = match.Groups[2].Value
                };

                conversationJson.lines.Add(lineJson);
            }
            else
            {
                Debug.LogError("failed to create conversation at line: " + line);
                Debug.LogError("aborting creating conversation");
                return;
            }
        }

        ConvoData convoData = new();
        string convoJson = JsonUtility.ToJson(conversationJson, true);
        EditorJsonUtility.FromJsonOverwrite(convoJson, convoData);
        
        // cannot create asset with colon symbol
        convoData.convoName = convoData.convoName.Replace(":", "");

        ConvoSO so = ScriptableObject.CreateInstance<ConvoSO>();
        so.data = convoData;

        AssetDatabase.CreateAsset(so, $"Assets/ScriptableObjects/Conversations/{convoData.convoName}.asset");
    }

    [Serializable]
    class ConversationJson
    {
        public string convoName;
        public List<LineJson> lines;
    }

    [Serializable]
    class LineJson
    {
        public string character;
        public string line;
    }
    
}
#endif