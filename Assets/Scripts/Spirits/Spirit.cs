using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    public static event System.Action<SpiritType> SpiritCollected;

    [SerializeField] SpiritType type;
    [SerializeField] float minLaunchSpeed;
    [SerializeField] float maxLaunchSpeed;
    [SerializeField] float launchDeceleration;
    [SerializeField] float minLaunchDuration;
    [SerializeField] float maxLaunchDuration;
    [SerializeField] float collectionAcceleration;
    [SerializeField] float minCollectionRange;
    [SerializeField] float maxCollectionRange;
    [SerializeField] float collectionRangeIncreaseRate;

    protected Rigidbody2D rb;
    protected bool isLaunching = false;
    protected bool isBeingCollected = false;
    protected float launchTimer;
    protected float collectionRange;

    public enum SpiritType
    {
        Blue,
        Red,
        Yellow,
        Pink
    }

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Start()
    {
        Vector3 dropDir = ((new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), 0f)).normalized + ((transform.position - PlayerID.instance.gameObject.transform.position).normalized * 1f)).normalized;
        rb.velocity = dropDir * Random.Range(minLaunchSpeed, maxLaunchSpeed);
        launchTimer = Random.Range(minLaunchDuration, maxLaunchDuration);
        isLaunching = true;
        collectionRange = minCollectionRange;
    }

    private void FixedUpdate()
    {
        if (isLaunching)
        {
            rb.velocity -= rb.velocity.normalized * launchDeceleration * Time.fixedDeltaTime; // Launching deceleration

            launchTimer -= Time.deltaTime;
            if (launchTimer <= 0f) // Launching ending 
            {
                isLaunching = false;
                rb.gravityScale = 0f;
                
                if (isBeingCollected)
                {
                    float collectionRangeScalar = Vector2.Distance(PlayerID.instance.gameObject.transform.position, transform.position) / 12f;
                    collectionRange = Mathf.Lerp(minCollectionRange, maxCollectionRange, collectionRangeScalar);
                }
            }
            return;
        }

        if (isBeingCollected)
        {
            Vector2 CollectionDir = (PlayerID.instance.gameObject.transform.position - transform.position).normalized;
            rb.velocity += CollectionDir * collectionAcceleration * Time.fixedDeltaTime; // Collection Acceleration
            if (collectionRange < maxCollectionRange) collectionRange += collectionRangeIncreaseRate * Time.fixedDeltaTime; // Collection Range Increase

            if (Vector2.Distance(PlayerID.instance.gameObject.transform.position, transform.position) <= collectionRange) // Spirit Collected
            {
                SpiritCollected?.Invoke(type);
                Destroy(gameObject);
            }
        }
        else if (rb.velocity.magnitude > 0.1f) // Base behavior: Decelerate to a stop
        {
            rb.velocity -= rb.velocity.normalized * launchDeceleration * Time.fixedDeltaTime;
            if (rb.velocity.magnitude <= 0.1f) rb.velocity = Vector2.zero;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.gameObject == PlayerID.instance.gameObject)
        {
            if (!isLaunching && !isBeingCollected)
            {
                float collectionRangeScalar = Vector2.Distance(PlayerID.instance.gameObject.transform.position, transform.position) / 12f;
                collectionRange = Mathf.Lerp(minCollectionRange, maxCollectionRange, collectionRangeScalar);
            }
            
            isBeingCollected = true;
        }
    }
}
