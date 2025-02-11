using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterSO", order = 1)]
public class CharacterSO : ScriptableObject
{
    public string characterName;

    [System.Serializable]
    public class ExpressionList
    {
        public string expressionName;
        public Sprite expressionSprite;
    }

    public List<ExpressionList> expressionList = new List<ExpressionList>();
}
