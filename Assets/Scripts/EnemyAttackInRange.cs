using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Allow for Enemy to detect enemy in melee range.
/// </summary>
public class EnemyAttackInRange : MonoBehaviour
{
    public bool inRange = false; // 

    void OnTriggerEnter2D(Collider2D col) {
        inRange = true;
    }

    void OnTriggerExit2D(Collider2D col) {
        inRange = false;
    }
}
