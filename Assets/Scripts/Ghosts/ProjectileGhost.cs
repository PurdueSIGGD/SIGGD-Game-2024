using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For Projectile Ghost GameObject
/// </summary>
public class ProjectileGhost : Ghost
{
    void Awake() {
        specialAction = new ProjectileAction();
    }
}
