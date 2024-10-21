using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Allow for Enemy to detect enemy in melee range.
/// </summary>
public class EnemyAttackInRange : MonoBehaviour
{
    public bool inRange = false; // Bool to pass if player in range
    public Collider2D PlayerCollider; // Used to Pass collider of player

    void OnTriggerEnter2D(Collider2D col) {
        if (col.name == "Test2DPlayer")  {
            PlayerCollider = col;
            inRange = true;
        }
      

    }

    void OnTriggerExit2D(Collider2D col) {
        inRange = false;
    }
}
