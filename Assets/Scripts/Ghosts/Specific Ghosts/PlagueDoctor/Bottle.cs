using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Bottle : MonoBehaviour, IStatList
{
    [SerializeField] private StatManager.Stat[] statList;
    [SerializeField] private GameObject blightPrefab;
    [SerializeField] private bool isMinibomb;

    public SilasManager manager;

    private bool hasCollided = false;



    void Start()
    {
        if (isMinibomb) StartCoroutine(HandleMinibomb());
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isMinibomb) return;
        if (hasCollided) return;
        hasCollided = true;

        // Affect Enemies
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, manager.GetStats().ComputeValue("Special Bomb Radius"), LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in enemiesHit)
        {
            enemy.transform.gameObject.GetComponent<Health>().Damage(manager.bombDamage, PlayerID.instance.gameObject);
            if (enemy.gameObject == null) continue;
            enemy.transform.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(Vector3.up, 3f, 0.3f);
            enemy.transform.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(enemy.transform.position - transform.position, 2f, 0.3f);
            if (enemy.GetComponentInChildren<BlightDebuff>() == null)
            {
                GameObject blight = Instantiate(manager.blightDebuff, enemy.transform);
                blight.GetComponent<BlightDebuff>().ApplyDebuff(manager);
            }
            else
            {
                enemy.GetComponentInChildren<BlightDebuff>().SetDebuffTime(manager.GetStats().ComputeValue("Blight Duration"));
            }
        }

        // Affect Player
        Collider2D playerHit = Physics2D.OverlapCircle(transform.position, manager.GetStats().ComputeValue("Special Bomb Radius"), LayerMask.GetMask("Player"));
        if (playerHit != null)
        {
            playerHit.transform.gameObject.GetComponent<Move>().ApplyKnockback(Vector3.up, 3f, false);
            playerHit.transform.gameObject.GetComponent<Move>().ApplyKnockback(playerHit.transform.position - transform.position, 2f, false);

            // Apply Self-medicated Buff
            SelfMedicated selfMedicated = manager.GetComponent<SelfMedicated>();
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
        GameObject explosion = Instantiate(manager.bombExplosionVFX, transform.position, Quaternion.identity);
        explosion.GetComponent<RingExplosionHandler>().playRingExplosion(manager.GetStats().ComputeValue("Special Bomb Radius"), manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);

        // Spawn Noxious Fumes poison cloud
        manager.GetComponent<NoxiousFumes>().SpawnPoisonCloud(transform.position, manager.GetStats().ComputeValue("Special Bomb Radius"), manager.GetStats().ComputeValue("Blight Duration"));
        
        // Spawn Minibombs
        for (int i = 0; i < manager.GetStats().ComputeValue("Special Minibomb Count"); i++)
        {
            // Calculate vector for ricochet
            Vector2 dir = (1.5f * (collision.gameObject.CompareTag("Enemy") ? Vector2.up : collision.GetContact(0).normal)) + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            GameObject bottle = Instantiate(manager.blightMinibomb, collision.GetContact(0).point + (dir * 0.25f), transform.rotation);
            bottle.GetComponent<Rigidbody2D>().velocity = dir.normalized * manager.GetStats().ComputeValue("Special Minibomb Speed");
            bottle.GetComponent<Bottle>().manager = manager;
        }

        Destroy(gameObject);
    }



    private IEnumerator HandleMinibomb()
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(Random.Range(manager.GetStats().ComputeValue("Special Minibomb Min Fuse Time"), manager.GetStats().ComputeValue("Special Minibomb Max Fuse Time")) - 0.1f);

        // Affect Enemies
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, manager.GetStats().ComputeValue("Special Minibomb Radius"), LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in enemiesHit)
        {
            enemy.transform.gameObject.GetComponent<Health>().Damage(manager.miniBombDamage, PlayerID.instance.gameObject);
            enemy.transform.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(Vector3.up, 1.5f, 0.3f);
            enemy.transform.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(enemy.transform.position - transform.position, 1f, 0.3f);
            if (enemy.GetComponentInChildren<BlightDebuff>() == null)
            {
                GameObject blight = Instantiate(manager.blightDebuff, enemy.transform);
                blight.GetComponent<BlightDebuff>().ApplyDebuff(manager);
            }
            else
            {
                enemy.GetComponentInChildren<BlightDebuff>().SetDebuffTime(manager.GetStats().ComputeValue("Blight Duration"));
            }
        }

        // Affect Player
        Collider2D playerHit = Physics2D.OverlapCircle(transform.position, manager.GetStats().ComputeValue("Special Minibomb Radius"), LayerMask.GetMask("Player"));
        if (playerHit != null)
        {
            playerHit.transform.gameObject.GetComponent<Move>().ApplyKnockback(Vector3.up, 1.5f, false);
            playerHit.transform.gameObject.GetComponent<Move>().ApplyKnockback(playerHit.transform.position - transform.position, 1f, false);

            // Apply Self-medicated Buff
            SelfMedicated selfMedicated = manager.GetComponent<SelfMedicated>();
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
        CameraShake.instance.Shake(0.075f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
        GameObject explosion = Instantiate(manager.bombExplosionVFX, transform.position, Quaternion.identity);
        explosion.GetComponent<RingExplosionHandler>().playRingExplosion(manager.GetStats().ComputeValue("Special Minibomb Radius"), manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);

        // Spawn Noxious Fumes poison cloud
        manager.GetComponent<NoxiousFumes>().SpawnPoisonCloud(transform.position, manager.GetStats().ComputeValue("Special Minibomb Radius"), manager.GetStats().ComputeValue("Blight Duration"));

        Destroy(gameObject);
    }



    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
