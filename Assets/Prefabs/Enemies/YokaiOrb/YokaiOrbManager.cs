using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YokaiOrbManager : EnemyStateManager
{
    [SerializeField] bool disableWhateverSpawningIsGoingOnHere = false;

    [SerializeField] private float spawnTime;
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private float enemySpawnSelfDamage;
    [SerializeField] private DamageContext enemySpawnSelfDamageContext;

    [Header("Custom Crow Tracking")]
    [SerializeField] float detectionRadius;

    private EnemySpawning enemySpawning;

    bool isWindingUp = false;

    private AudioSource windupSFX = null;

    private void OnEnable()
    {
        //GameplayEventHolder.OnDeathFilter.Add(OnKilled);
    }

    private void OnDisable()
    {
        if (GameplayEventHolder.OnDeathFilter.Contains(OnKilled)) GameplayEventHolder.OnDeathFilter.Remove(OnKilled);
    }

    void Start()
    {
        base.Start();
        MoveState = new YokaiMoveState(true);
        if (!disableWhateverSpawningIsGoingOnHere)
        {
            enemySpawning = PersistentData.Instance.GetComponent<EnemySpawning>();
            StartCoroutine(SpawnEnemy());
        }
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
        yield return new WaitForSeconds(spawnTime - 3f);
        GameplayEventHolder.OnDeathFilter.Add(OnKilled);
        isWindingUp = true;
        windupSFX = AudioManager.Instance.SFXBranch.PlaySFXTrack("YokaiRespawn");
        yield return new WaitForSeconds(3f);
        if (windupSFX != null) { windupSFX.Stop(); }
        //AudioManager.Instance.SFXBranch.PlaySFXTrack("RoombaExplosion");
        isWindingUp = false;
        GameObject nenemy = Instantiate(enemyToSpawn, transform.position, transform.rotation);
        enemySpawning.RegisterNewEnemy(nenemy);
        enemySpawnSelfDamageContext.damage = enemySpawnSelfDamage;
        GetComponent<Health>().Damage(enemySpawnSelfDamageContext, gameObject);
    }


    private void CancelWindup()
    {
        if (isWindingUp)
        {
            isWindingUp = false;
            if (windupSFX != null) { windupSFX.Stop(); }
        }
    }

    public void OnKilled(ref DamageContext context)
    {
        if (context.victim != gameObject) return;
        CancelWindup();
    }



    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
