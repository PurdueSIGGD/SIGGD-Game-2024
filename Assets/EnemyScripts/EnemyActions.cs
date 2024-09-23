using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyActions : MonoBehaviour
{
    [SerializeField] enemyAction detect;
    // Start is called before the first frame update
    void Start() {
       
    }

    void Update() {
        print(detect.inRange);
    }

    // Base Method
    /// <summary>
    /// Nothing yet implemented 
    /// </summary>
    void enemyAttack() {

    }
    
}
    
