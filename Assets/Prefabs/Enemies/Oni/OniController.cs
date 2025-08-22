using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniController : MonoBehaviour
{

    Health health;
    Rigidbody2D rb;
    Animator anim;
    GameObject targetObj;

    [Header("Flight params")]
    [SerializeField] float heightOffset; // how far up targetObject is from targetObject
    [SerializeField] float flightForce; // force of flight
    [SerializeField] float catchupFactor; // idk if this even does anything anymore
    [SerializeField] float repellFactor; // repelling force of targetObject on crow
    [SerializeField] float minRepell; // minimum distance for repelling force to take effect
    [SerializeField] float randomFactor; // force of random force
    [SerializeField] float maxSpeed;
    [SerializeField] float speedInc;

    [Header("Vision params")]
    [SerializeField] float visionRadius;
    [SerializeField] float trackMaxDistance;
    bool trackable;

    [Header("Attack params")]
    [SerializeField] DamageContext damageContext;
    [SerializeField] float damageVal;
    [SerializeField] float damageInc;
    [SerializeField] float gashRange;
    [Header("Heal params")]
    [SerializeField] HealingContext healingContext;

    bool gashable;

    void Awake()
    {
        GameplayEventHolder.OnDeath += OnDeath;
    }
    void OnDestroy()
    {
        GameplayEventHolder.OnDeath -= OnDeath;
    }
    void Start()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        healingContext.healing = health.GetStats().ComputeValue("Max Health");
        targetObj = null;
    }

    void Update()
    {
        Move(targetObj);
        if (targetObj == null)
            SetTarget(ScanForTargets());
        trackable = IsTargetInRange(targetObj);
        gashable = IsTargetGashable(targetObj);
        UpdateAnimatorParams();
    }
    void UpdateAnimatorParams()
    {
        anim.SetBool("trackable", trackable);
        anim.SetBool("gashable", gashable);
    }
    public void OnDeath(DamageContext context)
    {
        if (context.attacker != this.gameObject)
            return;

        SetTarget(null);
        KillEffects();
    }
    void KillEffects()
    {
        damageVal += damageInc;
        damageContext.damage = damageVal;
        maxSpeed += speedInc;
        health.Heal(healingContext, this.gameObject);
    }
    void OnGashTarget()
    {
        targetObj.GetComponent<Health>().Damage(damageContext, this.gameObject);
    }
    bool IsTargetGashable(GameObject targetObject)
    {
        if (targetObject == null)
            return false;
        Vector2 distance = targetObject.transform.position - transform.position;
        return distance.magnitude <= gashRange;
    }

    bool IsTargetInRange(GameObject targetObject)
    {
        if (targetObject == null)
            return false;
        Vector2 distance = targetObject.transform.position - transform.position;
        return distance.magnitude <= trackMaxDistance;
    }
    GameObject ScanForTargets()
    {
        List<GameObject> potentialTargets = new();
        int numRays = 16;
        for (float deg = 0; deg < (360 * Mathf.Deg2Rad); deg += 360 / numRays * Mathf.Deg2Rad)
        {
            // calculate unit vector direction based on angle
            Vector2 dir = new Vector2(Mathf.Cos(deg), Mathf.Sin(deg));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, visionRadius, LayerMask.GetMask("Player", "Enemy", "Ground"));
            Debug.DrawRay(transform.position, dir * visionRadius);
            if (hit && (hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("Enemy")))
            {
                potentialTargets.Add(hit.collider.gameObject);
            }
        }
        if (potentialTargets.Count == 0)
            return null;
        int randIndex = Random.Range(0, potentialTargets.Count);
        return potentialTargets[randIndex];
    }
    void Move(GameObject targetObject)
    {
        if (targetObject == null)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (targetObject.transform.position.x - transform.position.x < 0)
        {
            Flip(false);
        }
        else
        {
            Flip(true);
        }

        Vector2 targetPos = new Vector2(targetObject.transform.position.x, targetObject.transform.position.y) + Vector2.up * heightOffset;
        Vector2 current = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

        Vector2 directionRaw = targetPos - current;
        Vector2 direction = new Vector2(directionRaw.x, directionRaw.y).normalized;

        rb.AddForce(flightForce * direction, ForceMode2D.Impulse);

        // Slowing down as approaching targetObject y level
        float yDiff = targetPos.y - transform.position.y;
        yDiff = yDiff < 0.001f ? 0.1f : yDiff;
        rb.AddForce(1 / yDiff * catchupFactor * Vector2.up, ForceMode2D.Impulse);

        // BUT ALSO DON'T GET TOO CLOSE!!!
        Vector2 directionToPlayer = new Vector2(targetObject.transform.position.x - transform.position.x, targetObject.transform.position.y - transform.position.y);
        float distanceToPlayer = directionToPlayer.magnitude;
        if (distanceToPlayer < minRepell)
        {
            // avoid any division by zero
            distanceToPlayer = distanceToPlayer < 0.001f ? 0.1f : distanceToPlayer;
            float repellMagnitude = -1 / distanceToPlayer * repellFactor;
            if (repellMagnitude < -2)
            {
                repellMagnitude = -2;
            }
            rb.AddForce(repellMagnitude * directionToPlayer.normalized.x * Vector2.right, ForceMode2D.Impulse);
        }

        // RANDOM MOVEMENT!!!
        Vector2 random_vector = new Vector2(Random.value * 2 - 1, Random.value * 2 - 1);
        rb.AddForce(randomFactor * random_vector.normalized, ForceMode2D.Impulse);

        // CHECK MAX SPEED
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
    void Flip(bool isFlipped)
    {
        if (isFlipped)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180f, 0);
    }
    public GameObject GetTarget()
    {
        return targetObj;
    }
    public void SetTarget(GameObject targetObject)
    {
        this.targetObj = targetObject;
    }
}
