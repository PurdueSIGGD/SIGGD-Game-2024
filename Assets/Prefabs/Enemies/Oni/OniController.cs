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
    [SerializeField] GameObject gashVisual;
    bool gashable;

    [Header("Heal params")]
    [SerializeField] HealingContext healingContext;

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
        if (!gashable)
            gashVisual.SetActive(false);
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
        healingContext.healing = health.GetStats().ComputeValue("Max Health");
        health.Heal(healingContext, this.gameObject);
    }
    void OnGashTarget()
    {
        damageContext.damage = damageVal;
        targetObj.GetComponent<Health>().Damage(damageContext, this.gameObject);
        gashVisual.SetActive(true);
    }
    void OffGashTarget()
    {
        gashVisual.SetActive(false);
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
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, visionRadius, LayerMask.GetMask("Player", "Enemy"));
        List<GameObject> potentialTargets = new();
        foreach (Collider2D collision in collisions)
        {
            if (!collision.gameObject.name.Contains("Oni"))
            {
                potentialTargets.Add(collision.gameObject);
            }
        }
        // int numRays = 16;
        // for (float deg = 0; deg < (360 * Mathf.Deg2Rad); deg += 360 / numRays * Mathf.Deg2Rad)
        // {
        //     // calculate unit vector direction based on angle
        //     Vector2 dir = new Vector2(Mathf.Cos(deg), Mathf.Sin(deg));
        //     RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, visionRadius, LayerMask.GetMask("Player", "Enemy", "Ground"));
        //     Debug.DrawRay(transform.position, dir * visionRadius);
        //     if (hit && hit.collider.gameObject != this.gameObject)
        //     {
        //         if (hit.collider.gameObject.CompareTag("Player"))
        //         {
        //             return hit.collider.gameObject;
        //         }
        //         else if (hit.collider.gameObject.CompareTag("Enemy"))
        //         {
        //             potentialTargets.Add(hit.collider.gameObject);
        //         }
        //     }
        // }
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

        // CHECK MAX SPEED
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
    void Flip(bool isFlipped)
    {
        if (!isFlipped)
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
