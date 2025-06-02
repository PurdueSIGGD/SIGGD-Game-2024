using UnityEngine;

[System.Serializable]
public class ConvoData
{
    [System.Serializable]
    public class Line
    {
        public string character;
        public string expression;
        public string line;
        public AudioClip audio;
    }

    public string convoName;
    public Line[] lines;
}
