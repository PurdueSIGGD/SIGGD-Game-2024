using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningGhost : Ghost
{
    void Awake() {
        specialAction = new LightningAction();
    }
}
