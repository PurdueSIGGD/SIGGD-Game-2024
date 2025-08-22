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
    void Start()
    {
        base.Start();
        MoveState = new YokaiMoveState();
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
        Transform playerTransform = player.transform;
        GameObject skull = Instantiate(hitAndRunPrefab, playerTransform.position, Quaternion.identity);
        currentlyActiveAttacks.Add(skull);
        skull.GetComponent<ScatheHitAndRun>().Initialize(playerTransform, currentlyActiveAttacks.Remove);
    }
    public void Swipe()
    {
        int index = Random.Range(0, swipePositions.Count);
        Vector2 positionToSpawn = swipePositions[index].position;
        GameObject swipeObject = Instantiate(swipePrefab, positionToSpawn, Quaternion.identity);
        currentlyActiveAttacks.Add(swipeObject);
        swipeObject.GetComponent<ScatheSwipe>().Initialize(currentlyActiveAttacks.Remove);
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
}
