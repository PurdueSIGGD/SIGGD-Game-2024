using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyAction : MonoBehaviour
{
    public bool inRange = false;
    void OnTriggerEnter2D(Collider2D col) {
        inRange = true;
    }

    void OnTriggerExit2D(Collider2D col) {
        inRange = false;
    }


}
