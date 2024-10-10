using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For Invisibility Ghost GameObject
/// </summary>
public class InvisibilityGhost : Ghost
{
    void Awake() {
        specialAction = new InvisibleAction();
    }
}
