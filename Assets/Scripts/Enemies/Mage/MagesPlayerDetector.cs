using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagesPlayerDetector : MonoBehaviour
{
    public event Action<GameObject> OnPlayerEnteredRange;   // passes the player GameObject
    public event Action<GameObject> OnPlayerExitedRange;   // passes the player GameObject

    private CircleCollider2D rangeCollider;


    private void Awake()
    {
        rangeCollider = GetComponent<CircleCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPlayerEnteredRange?.Invoke(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPlayerExitedRange?.Invoke(collision.gameObject);
        }
    }

    public void SetRange(float range)
    {
        rangeCollider.radius = range;
    }
}
