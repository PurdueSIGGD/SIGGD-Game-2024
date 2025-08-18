using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Epidemic : Skill
{
    [SerializeField]
    List<float> values = new List<float>
    {
        0f, 1f, 2f, 3f, 4f
    };
    private int pointIndex;

    [SerializeField] public DamageContext blastDamage;

    private SilasManager manager;



    private void OnEnable()
    {
        GameplayEventHolder.OnDeath += BlightedEnemyOnDeath;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDeath -= BlightedEnemyOnDeath;
    }



    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<SilasManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void BlightedEnemyOnDeath(DamageContext context)
    {
        if (pointIndex <= 0) return;
        if (!context.victim.CompareTag("Enemy") || context.victim.GetComponentInChildren<BlightDebuff>() == null) return;

        StartCoroutine(DeathExplosionCoroutine(context, context.victim.transform.position));
    }



    private IEnumerator DeathExplosionCoroutine(DamageContext context, Vector3 position)
    {
        yield return new WaitForSeconds(0.1f);

        // Affect Enemies
        blastDamage.damage = manager.GetStats().ComputeValue("Blight Epidemic Blast Damage");
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(position, manager.GetStats().ComputeValue("Blight Epidemic Blast Radius"), LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in enemiesHit)
        {
            //if (enemy.transform.gameObject.Equals(context.victim)) continue;
            enemy.transform.gameObject.GetComponent<Health>().Damage(blastDamage, PlayerID.instance.gameObject);
            if (enemy.gameObject == null) continue;
            enemy.transform.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(Vector3.up, 3f, 0.3f);
            enemy.transform.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(enemy.transform.position - position, 2f, 0.3f);
            if (enemy.GetComponentInChildren<BlightDebuff>() == null)
            {
                GameObject blight = Instantiate(manager.blightDebuff, enemy.transform);
                blight.GetComponent<BlightDebuff>().ApplyDebuff(manager, values[pointIndex]); //TODO: Override duration time
            }
            else
            {
                enemy.GetComponentInChildren<BlightDebuff>().AddDebuffTime(values[pointIndex]);
            }
        }

        // Affect Player
        Collider2D playerHit = Physics2D.OverlapCircle(position, manager.GetStats().ComputeValue("Blight Epidemic Blast Radius"), LayerMask.GetMask("Player"));
        if (playerHit != null)
        {
            playerHit.transform.gameObject.GetComponent<Move>().ApplyKnockback(Vector3.up, 3f, false);
            playerHit.transform.gameObject.GetComponent<Move>().ApplyKnockback(playerHit.transform.position - position, 2f, false);

            // Apply Self-medicated Buff
            SelfMedicated selfMedicated = GetComponent<SelfMedicated>();
            if (selfMedicated.isBuffed)
            {
                selfMedicated.SetBuffTime(selfMedicated.blightBuffDuration);
            }
            else
            {
                selfMedicated.ApplyBuff(selfMedicated.blightBuffDuration);
            }
        }

        // VFX
        CameraShake.instance.Shake(0.15f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
        GameObject explosion = Instantiate(manager.bombExplosionVFX, position, Quaternion.identity);
        explosion.GetComponent<RingExplosionHandler>().playRingExplosion(manager.GetStats().ComputeValue("Blight Epidemic Blast Radius"), manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);

        // Spawn Noxious Fumes poison cloud
        manager.GetComponent<NoxiousFumes>().SpawnPoisonCloud(position, manager.GetStats().ComputeValue("Blight Epidemic Blast Radius"), values[pointIndex]);
    }



    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void ClearPointsTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
    }
}
