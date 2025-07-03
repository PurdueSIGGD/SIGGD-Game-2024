using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for Shielded Riot Police's shield
/// </summary>
public class PoliceShield : MonoBehaviour
{
    ShieldPolice shieldPolice;

    [SerializeField] float damageVal;
    void Start()
    {
        shieldPolice = this.gameObject.GetComponentInParent<ShieldPolice>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        shieldPolice.ProcessShieldCollision(collision);
        print("SHIELD: COLLISION");
    }
}
