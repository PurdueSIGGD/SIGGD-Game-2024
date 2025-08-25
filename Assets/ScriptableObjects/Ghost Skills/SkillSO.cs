using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillSO", order = 1)]
public class SkillSO : ScriptableObject
{
    public string skillName;
    public Sprite sprite;
    [TextArea(4,4)] public string description;
    [TextArea(2, 2)] public string descriptionValue;
    public string levelOneValue;
    public string levelTwoValue;
    public string levelThreeValue;
    public string levelFourValue;
}
