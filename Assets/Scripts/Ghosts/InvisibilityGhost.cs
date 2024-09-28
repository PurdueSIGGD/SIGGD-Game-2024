using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibilityGhost : Ghost
{
    void Awake() {
        specialAction = new InvisibleAction();
    }
}
