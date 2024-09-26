using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyActions : MonoBehaviour
{
    [SerializeField] enemyAction detect;
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

    // Base Method
    /// 
    /// Nothing yet implemented 
    /// Basic Print Statement
    /// 
    void enemyAttack() {
        print("ATTACK!");
    }
    
}
    
