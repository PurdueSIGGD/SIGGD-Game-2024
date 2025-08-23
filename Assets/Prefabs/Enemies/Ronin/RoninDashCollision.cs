using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoninDashCollision : MonoBehaviour
{
    [SerializeField] Ronin ronin;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ronin.OnDashHit(collision.collider);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ronin.OnDashHit(collision);
    }
}
