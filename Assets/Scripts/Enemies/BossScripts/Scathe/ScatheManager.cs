using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatheManager : EnemyStateManager
{

    List<GameObject> currentlyActiveAttacks = new List<GameObject>();
    [Header("Custom Crow Tracking")]
    [SerializeField] float detectionRadius;
    [Header("Attack Prefabs")]
    [SerializeField] GameObject hitAndRunPrefab;
    [SerializeField] GameObject swipePrefab;
    [SerializeField] GameObject swipePositionsHolder;
    List<Transform> swipePositions = new List<Transform>();



    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += OnPlayerDamageTaken;
        GameplayEventHolder.OnDeath += OnDeath;
        GameplayEventHolder.OnDamageDealt += OnDamageTaken;
        GameplayEventHolder.OnDeath += OnPlayerDeath;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= OnPlayerDamageTaken;
        GameplayEventHolder.OnDeath -= OnDeath;
        GameplayEventHolder.OnDamageDealt -= OnDamageTaken;
        GameplayEventHolder.OnDeath -= OnPlayerDeath;
    }

    void Start()
    {
        base.Start();
        MoveState = new ScatheMoveState();
        swipePositions = new(swipePositionsHolder.GetComponentsInChildren<Transform>(includeInactive: false));
        swipePositions.Remove(swipePositionsHolder.transform);
    }

    public override bool HasLineOfSight(bool tracking)
    {
        bool hit_player = false;

        Vector2 dir = transform.TransformDirection(Vector2.right);
        float maxDistance = detectionRadius;

        // track player if player is being tracked
        if (tracking)
        {
            maxDistance = maxDistance * 1.2f;
            float maxTrackDistance = maxDistance * 2;

            dir = player.position - transform.position;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxTrackDistance, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, dir);
            if (hit && hit.collider.gameObject.CompareTag("Player"))
            {
                hit_player = true;
            }
        }

        // if not tracking player
        // casts numRays rays in a circle to seek player
        int numRays = 16;
        for (float deg = 0; deg < (360 * Mathf.Deg2Rad); deg += 360 / numRays * Mathf.Deg2Rad)
        {
            // calculate unit vector direction based on angle
            dir = new Vector2(Mathf.Cos(deg), Mathf.Sin(deg));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxDistance, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, dir * maxDistance);
            if (hit && hit.collider.gameObject.CompareTag("Player"))
            {
                hit_player = true;
            }
        }
        return hit_player;
    }

    public void HitAndRun()
    {
        AudioManager.Instance.VABranch.PlayVATrack("Scathe Dash");
        Transform playerTransform = player.transform;
        GameObject skull = Instantiate(hitAndRunPrefab, playerTransform.position, Quaternion.identity);
        currentlyActiveAttacks.Add(skull);
        skull.GetComponent<ScatheHitAndRun>().Initialize(playerTransform, currentlyActiveAttacks.Remove);
    }
    public void Swipe()
    {
        //int index = Random.Range(0, swipePositions.Count);
        int index = GetClosestSwipeIndexToPlayer();
        Vector2 positionToSpawn = swipePositions[index].position;
        GameObject swipeObject = Instantiate(swipePrefab, positionToSpawn, Quaternion.identity);
        currentlyActiveAttacks.Add(swipeObject);
        swipeObject.GetComponent<ScatheSwipe>().Initialize(currentlyActiveAttacks.Remove);
    }

    private int GetClosestSwipeIndexToPlayer()
    {
        Vector2 playerPosition = player.position;
        int index = 0;
        float minDistance = 9999f;
        for (int i = 0; i < swipePositions.Count; i++)
        {
            float distance = Vector2.Distance(playerPosition, swipePositions[i].position);
            if (distance < minDistance)
            {
                index = i;
                minDistance = distance;
            }
        }
        return index;
    }

    public List<GameObject> GetActiveAttacks()
    {
        return currentlyActiveAttacks;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    protected override void OnFinishAnimation()
    {
        base.OnFinishAnimation();
    }



    public void OnDamageTaken(DamageContext context)
    {
        if (context.victim != gameObject || context.damage <= 0f) return;

        if (context.extraContext.Equals("GIANT SCATHE SKULL"))
        {
            AudioManager.Instance.VABranch.PlayVATrack("Scathe Significant Damage Taken");
            return;
        }
        AudioManager.Instance.VABranch.PlayVATrack("Scathe Light Damage Taken");
    }

    public void OnDeath(DamageContext context)
    {
        if (context.victim != gameObject) return;

        Debug.Log("I KILLED SCATHE WOT");
        //AudioManager.Instance.VABranch.PlayVATrack("Scathe On Boss Death");
        PlayVoiceLineDelayed("Scathe On Boss Death", 0.5f);
    }

    public void OnPlayerDamageTaken(DamageContext context)
    {
        if (!context.victim.CompareTag("Player") || context.damage <= 0f) return;
        if (!(context.extraContext.Equals("GIANT SCATHE SKULL") || context.extraContext.Equals("SCATHE TAIL SWIPE"))) return;
        if (context.victim.GetComponent<Health>().currentHealth <= 0f) return;

        Debug.Log("SCATHE ATTACK HAPPENED OUCH");
        AudioManager.Instance.VABranch.PlayVATrack("Scathe Damaging Player");
    }

    public void OnPlayerDeath(DamageContext context)
    {
        if (!context.victim.CompareTag("Player")) return;

        //AudioManager.Instance.VABranch.PlayVATrack("Scathe Player Death");
        PlayVoiceLineDelayed("Scathe Player Death", 1f);
    }

    private void PlayVoiceLineDelayed(string lineName, float delay)
    {
        StartCoroutine(PlayVoiceLineDelayedCoroutine(lineName, delay));
    }

    private IEnumerator PlayVoiceLineDelayedCoroutine(string lineName, float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.VABranch.PlayVATrack(lineName);
    }
}
