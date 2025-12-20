using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A type of a an Enemy Projectile. Will track the player until a certain distance
/// away from it.
/// </summary>
public class TrackingProjectile : EnemyProjectile
{
    SpriteRenderer sprite;
    [SerializeField] float trackingStrength; // A larger value will allow the projectile to turn faster.
    [SerializeField] float trackingDistance; // A larger value will lead the projectile to loose tracking earlier
    private float hangTime = 0;
    private bool tracking = true;
    protected Transform player;
    private float baseScale;
    [SerializeField] float explosionDeceleration;
    [SerializeField] float explosionWindUpTime;
    [SerializeField] float explosionRadius;

    [SerializeField] GameObject explodeMarker;
    [SerializeField] GameObject explodeVisual;
    bool toggleRed = true;
    bool isExploding = false;

    //Consistently tracks player
    protected override void Start()
    {
        base.Start();
        sprite = GetComponent<SpriteRenderer>();
        player = PlayerID.instance.transform;
        Vector3 directionToTarget = (player.position - transform.position).normalized;
        rb.velocity = directionToTarget * speed;
        baseScale = transform.localScale.x;
    }
    void Update()
    {
        if (PlayerID.instance.gameObject.transform.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(baseScale, baseScale, baseScale);
        }
        else
        {
            transform.localScale = new Vector3(-baseScale, baseScale, baseScale);
        }
    }

    void FixedUpdate()
    {   // Stops tracking within certain distance of player
        if (Vector3.Distance(transform.position, player.position) <= trackingDistance && tracking)
        {
            InitiateExplosion();
        }
        Move();
        CheckOutOfBounds(Time.deltaTime);
    }

    // Moves the projectile
    public new void Move()
    {
        if (tracking)
        {
            Vector3 directionToTarget = (player.position - transform.position).normalized;
            //Quaternion rotation = Quaternion.LookRotation(directionToTarget);
            //rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, trackingStrength));
            rb.velocity = directionToTarget * speed;
        }
        else
        {
            speed -= Time.deltaTime * explosionDeceleration;
            speed = Mathf.Max(speed, 0f);
            rb.velocity = rb.velocity.normalized * speed;
        }
    }

    // Deletes the projectile if it has existed for a certain amount of time
    private void CheckOutOfBounds(float dt)
    {
        hangTime += dt;
        if (hangTime > range && !isExploding)
        {
            if (tracking)
            {
                InitiateExplosion();
                return;
            }
            isExploding = true;
            StopAllCoroutines();
            StartCoroutine(Explode());
        }
    }



    IEnumerator Flicker()
    {
        while (true)
        {
            explodeMarker.SetActive(toggleRed);
            if (toggleRed)
            {
                sprite.color = Color.magenta;
                toggleRed = false;
            }
            else
            {
                sprite.color = Color.white;
                toggleRed = true;
            }
            yield return new WaitForSeconds(0.05f);
            yield return null;
        }
    }

    private void InitiateExplosion()
    {
        tracking = false;
        StartCoroutine(Flicker());
        hangTime = 0f;
        range = explosionWindUpTime;
    }

    IEnumerator Explode()
    {
        explodeMarker.SetActive(false);
        explodeVisual.SetActive(true);
        GenerateDamageFrame(transform.position, explosionRadius, projectileDamage, gameObject);
        yield return new WaitForSeconds(0.15f);
        GetComponent<Health>().Damage(projectileDamage, this.gameObject);
        //Destroy(gameObject);
    }


    protected bool GenerateDamageFrame(Vector2 pos, float radius, DamageContext damageContext, GameObject attacker /*float damage*/)
    {
        // Check for player to do damage
        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, radius, LayerMask.GetMask("Player", "Idol_Clone"));
        foreach (Collider2D hit in hits)
        {
            if (hit)
            {
                float dmgDealt = hit.GetComponent<Health>().Damage(damageContext, attacker);
                if (hit.CompareTag("Player") && dmgDealt > 0)
                {
                    PlayerID.instance.GetComponent<PlayerStateMachine>().SetStun(0.2f);
                }
            }
        }
        return (hits.Length > 0);
    }
}
