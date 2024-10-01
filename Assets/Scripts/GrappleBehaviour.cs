using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class GrappleBehaviour : MonoBehaviour
{
    [SerializeField] float speed = 10;

    private Vector2 dir = Vector2.zero;
    private Rigidbody2D rb = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = dir * speed;
    }
}
