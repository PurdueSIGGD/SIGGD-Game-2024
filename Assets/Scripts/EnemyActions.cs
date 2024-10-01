using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Enemy default attack behavior
/// </summary>
public class EnemyActions : MonoBehaviour
{
    [SerializeField] EnemyAttackInRange detect;
    public float attackSpeed;
    private float attackTimer = 0f; 
    // Start is called before the first frame update
    void Start() {
       
    }

    void Update() {

        if (detect.inRange) {
            // If in attackRange than run attack timer
            attackTimer += Time.deltaTime;
        }
        if (attackTimer >= attackSpeed) {
            // Once AttackTime equals AttackSpeed then attack
            enemyAttack();
            // Resets Timer
            attackTimer = 0f;
        }
    }

    /// <summary>
    /// Makes the enemy perform default attack
    /// </summary>
    void enemyAttack() {
        print("ATTACK!");
    }
    
}
    
