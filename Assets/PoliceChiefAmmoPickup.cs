using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceChiefAmmoPickup : MonoBehaviour
{
    [SerializeField] private float launchDuration;
    [SerializeField] private float collectionAcceleration;
    [SerializeField] private float collectionJerkPerFrame;
    [SerializeField] private float collectionRadius;
    [SerializeField] private LayerMask collectionCollisionExclusionLayerMask;

    private PoliceChiefManager manager;
    private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider;
    private GameObject player;

    private bool isLaunching;
    private float launchTimer;
    private bool isCollecting;

    // Start is called before the first frame update
    void Start()
    {
        launchTimer = launchDuration;
        isLaunching = true;
        isCollecting = false;
        rb = GetComponent<Rigidbody2D>();
        player = PlayerID.instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.autoRecoverAmmo && !isCollecting)
        {
            StartCollection();
        }

        if (isLaunching)
        {
            launchTimer -= Time.deltaTime;
            if (launchTimer <= 0f)
            {
                launchTimer = 0f;
                isLaunching = false;
            }
        }
        
        if (isCollecting)
        {
            collectionAcceleration *= collectionJerkPerFrame;
            float frameAcceleration = collectionAcceleration * Time.deltaTime;
            Vector2 pickUpToPlayerDir = Vector3.Normalize(player.transform.position - transform.position);
            rb.velocity += (frameAcceleration * pickUpToPlayerDir);

            if (Vector2.Distance(player.transform.position, transform.position) <= collectionRadius)
            {
                CollectPickup();
            }
        }
    }

    public void InitializeAmmoPickup(PoliceChiefManager manager, Vector3 initialVelocity)
    {
        this.manager = manager;
        GetComponent<Rigidbody2D>().velocity = initialVelocity;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isLaunching || isCollecting) return;
        if (collision.transform.gameObject == player) StartCollection();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCollecting) return;
        if (collision.gameObject.CompareTag("Boundary")) StartCollection();
    }

    public void StartCollection()
    {
        if (isCollecting) return;
        launchTimer = 0f;
        isLaunching = false;
        isCollecting = true;
        rb.gravityScale = 0f;
        boxCollider.excludeLayers = collectionCollisionExclusionLayerMask;
    }

    private void CollectPickup()
    {
        if (manager == null) return;
        manager.basic.AddAmmo(1);
        AudioManager.Instance.SFXBranch.GetSFXTrack("North-Ricochet").SetPitch(Mathf.Max(manager.basicAmmo - 1, 0), Mathf.Max(manager.GetStats().ComputeValue("Basic Starting Ammo") - 1, 0));
        AudioManager.Instance.SFXBranch.PlaySFXTrack("North-Ricochet");
        Destroy(gameObject);
    }
}
