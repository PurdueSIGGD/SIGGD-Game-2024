using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoliceChiefSidearmShot : MonoBehaviour
{
    private PoliceChiefManager manager;
    private bool isDoubleTap = false;
    private bool isPowerSpike = false;
    private float travelSpeed = 300f;
    public static event System.Action enemyWasShot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fireSidearmShot(PoliceChiefManager manager, Vector2 pos, Vector2 dir)
    {
        fireSidearmShot(manager, pos, dir, false, false);
    }

    public void fireSidearmShot(PoliceChiefManager manager, Vector2 pos, Vector2 dir, bool isDoubleTap)
    {
        //this.manager = manager;
        //this.isDoubleTap = isDoubleTap;
        //StartCoroutine(sidearmShotCoroutine(pos, dir));
        fireSidearmShot(manager, pos, dir, isDoubleTap, false);
    }

    public void fireSidearmShot(PoliceChiefManager manager, Vector2 pos, Vector2 dir, bool isDoubleTap, bool isPowerSpike)
    {
        this.manager = manager;
        this.isDoubleTap = isDoubleTap;
        this.isPowerSpike = isPowerSpike;
        StartCoroutine(sidearmShotCoroutine(pos, dir));
    }

    private RaycastHit2D rayCastDetection(Vector2 pos, Vector2 dir, float distToTravel)
    {
        if (distToTravel <= 0)
        {
            return new RaycastHit2D();
        }
        RaycastHit2D hit = Physics2D.Raycast(pos, dir, distToTravel, LayerMask.GetMask("Ground", "Enemy"));
        if (!hit)
        {
            Debug.DrawLine(pos, pos + dir * distToTravel, Random.ColorHSV(), 5f);
            return hit;
        }
        Debug.DrawLine(pos, hit.point, Random.ColorHSV(), 5f);
        return hit;
    }

    private IEnumerator sidearmShotCoroutine(Vector2 pos, Vector2 dir)
    {
        // Calculate shot vector
        RaycastHit2D hit = rayCastDetection(pos, dir, manager.GetStats().ComputeValue("Basic Travel Distance"));

        Vector2 hitPoint = (hit) ? hit.point : pos + (dir * manager.GetStats().ComputeValue("Basic Travel Distance"));



        // VFX
        //Debug.DrawLine(pos, hitPoint, Color.red, 5.0f);
        CameraShake.instance.Shake(0.25f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
        GameObject railgunTracer = Instantiate(manager.basicTracerVFX, Vector3.zero, Quaternion.identity);
        Color tracerColor = (manager.GetComponent<PoliceChiefLethalForce>().shotEmpowered && !isDoubleTap) ? manager.GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor : manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor;
        railgunTracer.GetComponent<RaycastTracerHandler>().playTracer(pos, hitPoint, travelSpeed, tracerColor, tracerColor);

        // Wait for travel speed
        yield return new WaitForSeconds(Vector2.Distance(pos, hitPoint) / travelSpeed);

        // No Hit Ammo Pickup
        if (!hit)
        {
            if (isPowerSpike)
            {
                Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(hitPoint, manager.GetComponent<PoliceChiefPowerSpike>().explosionRadius, LayerMask.GetMask("Enemy"));
                foreach (Collider2D enemy in enemiesHit)
                {
                    enemy.transform.gameObject.GetComponent<Health>().Damage(manager.GetComponent<PoliceChiefPowerSpike>().GetExplosionDamage(), PlayerID.instance.gameObject);
                }
                if (enemiesHit.Length > 0) enemyWasShot?.Invoke();
                GameObject airExplosion = Instantiate(manager.basicImpactExplosionVFX, hitPoint, Quaternion.identity);
                airExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(manager.GetComponent<PoliceChiefPowerSpike>().explosionRadius, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
            }
            if (!isDoubleTap)
            {
                GameObject airAmmoPickup = Instantiate(manager.basicAmmoPickup, hitPoint, Quaternion.identity);
                airAmmoPickup.GetComponent<PoliceChiefAmmoPickup>().InitializeAmmoPickup(manager, dir * 5f);
            }
            manager.GetComponent<PoliceChiefPowerSpike>().StopCritTimer();
            Destroy(this.gameObject);
            yield break;
        }

        // Affect enemies
        if (hit.transform.CompareTag("Enemy"))
        {
            if (isPowerSpike)
            {
                Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(hit.point, manager.GetComponent<PoliceChiefPowerSpike>().explosionRadius, LayerMask.GetMask("Enemy"));
                foreach (Collider2D enemy in enemiesHit)
                {
                    enemy.transform.gameObject.GetComponent<Health>().Damage(manager.GetComponent<PoliceChiefPowerSpike>().GetExplosionDamage(), PlayerID.instance.gameObject);
                }
            }
            hit.transform.gameObject.GetComponent<Health>().Damage((isDoubleTap) ? manager.GetComponent<DoubleTapSkill>().secondaryShotDamage : manager.basicDamage, PlayerID.instance.gameObject);
            enemyWasShot?.Invoke();
            GameObject enemyExplosion = Instantiate(manager.basicImpactExplosionVFX, hit.point, Quaternion.identity);
            enemyExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(((isPowerSpike) ? manager.GetComponent<PoliceChiefPowerSpike>().explosionRadius : 1f), manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
            manager.GetComponent<PoliceChiefPowerSpike>().StopCritTimer();
            Destroy(this.gameObject);
            yield break;
        }

        // Surface impact Ammo Pickup & VFX
        if (isPowerSpike)
        {
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(hit.point, manager.GetComponent<PoliceChiefPowerSpike>().explosionRadius, LayerMask.GetMask("Enemy"));
            foreach (Collider2D enemy in enemiesHit)
            {
                enemy.transform.gameObject.GetComponent<Health>().Damage(manager.GetComponent<PoliceChiefPowerSpike>().GetExplosionDamage(), PlayerID.instance.gameObject);
            }
            if (enemiesHit.Length > 0) enemyWasShot?.Invoke();
        }
        if (!isDoubleTap)
        {
            Vector2 reflect = Vector2.Reflect(dir, hit.normal);
            GameObject surfaceAmmoPickup = Instantiate(manager.basicAmmoPickup, hit.point + new Vector2(reflect.x * 0.1f, reflect.y * 0.1f), Quaternion.identity);
            surfaceAmmoPickup.GetComponent<PoliceChiefAmmoPickup>().InitializeAmmoPickup(manager, reflect * 10f);
        }
        GameObject surfaceExplosion = Instantiate(manager.basicImpactExplosionVFX, hit.point, Quaternion.identity);
        surfaceExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(((isPowerSpike) ? manager.GetComponent<PoliceChiefPowerSpike>().explosionRadius : 0.5f), manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
        manager.GetComponent<PoliceChiefPowerSpike>().StopCritTimer();
        Destroy(this.gameObject);
    }
}
