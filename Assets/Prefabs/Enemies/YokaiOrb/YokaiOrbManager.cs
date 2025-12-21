using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YokaiOrbManager : EnemyStateManager
{
    [SerializeField] private float spawnTime;
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private float enemySpawnSelfDamage;
    [SerializeField] private DamageContext enemySpawnSelfDamageContext;

    [Header("Custom Crow Tracking")]
    [SerializeField] float detectionRadius;

    private EnemySpawning enemySpawning;

    void Start()
    {
        base.Start();
        MoveState = new YokaiMoveState(true);
        enemySpawning = PersistentData.Instance.GetComponent<EnemySpawning>();
        StartCoroutine(SpawnEnemy());
    }



    public void SetEnemyToSpawn(GameObject enemyToSpawn)
    {
        this.enemyToSpawn = enemyToSpawn;
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

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxTrackDistance, LayerMask.GetMask("Player", "Ground"));
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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxDistance, LayerMask.GetMask("Player", "Ground"));
            Debug.DrawRay(transform.position, dir * maxDistance);
            if (hit && hit.collider.gameObject.CompareTag("Player"))
            {
                hit_player = true;
            }
        }
        return hit_player;
    }



    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(spawnTime);
        GameObject nenemy = Instantiate(enemyToSpawn, transform.position, transform.rotation);
        enemySpawning.RegisterNewEnemy(nenemy);
        enemySpawnSelfDamageContext.damage = enemySpawnSelfDamage;
        GetComponent<Health>().Damage(enemySpawnSelfDamageContext, gameObject);
    }



    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
