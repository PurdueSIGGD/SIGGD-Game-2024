using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterSO", order = 1)]
public class CharacterSO : ScriptableObject
{
    public string displayName;

    public Sprite characterIcon;

    public Sprite hudIcon;

    public float hudIconYOffset;

    public Color primaryColor;

    public Color highlightColor;

    public Color whiteColor;

    public Sprite fullImage;

    [Header("Abilities")]

    public Sprite basicAbilityIcon;

    public PlayerActionInput basicAbilityInput;

    public string basicAbilityName;

    [TextArea]
    public string basicAbilityDescription;

    public Sprite specialAbilityIcon;

    public PlayerActionInput specialAbilityInput;

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

public enum PlayerActionInput
{
    NONE,
    LEFT_CLICK,
    RIGHT_CLICK,
    LEFT_SHIFT,
    S,
}
