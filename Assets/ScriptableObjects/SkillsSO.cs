using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillsSO", order = 1)]
public class SkillsSO : ScriptableObject
{
    public string skillName;
    public Sprite sprite;
    public string description;
    public float[] specialNumbers;
}
