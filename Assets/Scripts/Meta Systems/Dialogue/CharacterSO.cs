using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterSO", order = 1)]
public class CharacterSO : ScriptableObject
{
    public string displayName;

    public Sprite characterIcon;

    public Color primaryColor;

    public Sprite fullImage;

    [Header("Abilities")]

    public Sprite basicAbilityIcon;

    public string basicAbilityName;

    [TextArea]
    public string basicAbilityDescription;

    public Sprite specialAbilityIcon;

    public string specialAbilityName;

    [TextArea]
    public string specialAbilityDescription;


    [System.Serializable]
    public class ExpressionList
    {
        public string expressionName;
        public Sprite expressionSprite;
    }

    public List<ExpressionList> expressionList = new List<ExpressionList>();
}
