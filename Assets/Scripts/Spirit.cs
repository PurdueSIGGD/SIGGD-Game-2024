using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    [SerializeField] SpiritType type;
    [SerializeField] float collectionSpeed;
    protected Rigidbody2D rb;
    enum SpiritType
    {
        Blue,
        Red,
        Yellow,
        Pink
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Do something
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        transform.position = Vector2.MoveTowards(transform.position, collision.transform.position, collectionSpeed);

    }
}
