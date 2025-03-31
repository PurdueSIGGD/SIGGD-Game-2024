using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterSO", order = 1)]
public class CharacterSO : ScriptableObject
{
    public string displayName;

    public Sprite characterIcon;

    public Color primaryColor;

    public Sprite basicAbilityIcon;

    public Sprite specialAbilityIcon;

    [System.Serializable]
    public class ExpressionList
    {
        public string expressionName;
        public Sprite expressionSprite;
    }

    public List<ExpressionList> expressionList = new List<ExpressionList>();
}
