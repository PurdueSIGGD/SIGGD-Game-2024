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
}
