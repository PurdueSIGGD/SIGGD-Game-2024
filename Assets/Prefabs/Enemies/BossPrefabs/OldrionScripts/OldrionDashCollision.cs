using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldrionDashCollision : MonoBehaviour
{
    [SerializeField] OldrionManager oldrion;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        oldrion.OnDashHit(collision.collider);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        oldrion.OnDashHit(collision);
    }
}
