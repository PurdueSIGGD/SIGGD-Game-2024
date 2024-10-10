using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For Lightning Ghost GameObject
/// </summary>
public class LightningGhost : Ghost
{
    void Awake() {
        specialAction = new LightningAction();
    }
}
