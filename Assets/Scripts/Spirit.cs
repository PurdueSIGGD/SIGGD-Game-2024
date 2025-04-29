using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    public static event System.Action<SpiritType> SpiritCollected;

    [SerializeField] SpiritType type;
    [SerializeField] float collectionSpeed;
    protected Rigidbody2D rb;
    public enum SpiritType
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
            SpiritCollected?.Invoke(type);
            Destroy(gameObject);
        }
    }

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.transform.gameObject == PlayerID.instance.gameObject)
        {
            transform.position = Vector2.MoveTowards(transform.position, collision.transform.position, collectionSpeed);
        }

    }
}
