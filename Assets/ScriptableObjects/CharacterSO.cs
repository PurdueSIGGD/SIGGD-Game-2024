using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterSO", order = 1)]
public class CharacterSO : ScriptableObject
{
    public string charactername;
    public Sprite sprite;
}
