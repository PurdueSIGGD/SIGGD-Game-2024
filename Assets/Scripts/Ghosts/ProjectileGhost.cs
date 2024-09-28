using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGhost : Ghost
{
    void Awake() {
        specialAction = new ProjectileAction();
    }
}
