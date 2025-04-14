using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ConversationSO", order = 1)]
public class ConversationSO : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        public CharacterSO character;
        public string expression;
        [TextArea(5, 3)]
        public string line;
    }

    public DialogueLine[] dialogueLines;
}
