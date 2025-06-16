using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceChiefSidearmShot : MonoBehaviour
{
    private PoliceChiefManager manager;
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
        this.manager = manager;
        StartCoroutine(sidearmShotCoroutine(pos, dir));
        GameplayEventHolder.OnAbilityUsed?.Invoke(manager.sidearmActionContext);
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
        railgunTracer.GetComponent<RaycastTracerHandler>().playTracer(pos, hitPoint, travelSpeed, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);

        // Wait for travel speed
        yield return new WaitForSeconds(Vector2.Distance(pos, hitPoint) / travelSpeed);

        // No Hit Ammo Pickup
        if (!hit)
        {
            GameObject airAmmoPickup = Instantiate(manager.basicAmmoPickup, hitPoint, Quaternion.identity);
            airAmmoPickup.GetComponent<PoliceChiefAmmoPickup>().InitializeAmmoPickup(manager, dir * 5f);
            Destroy(this.gameObject);
            yield break;
        }

        // Affect enemies
        if (hit.transform.CompareTag("Enemy"))
        {
            hit.transform.gameObject.GetComponent<Health>().Damage(manager.basicDamage, PlayerID.instance.gameObject);
            enemyWasShot?.Invoke();
            GameObject enemyExplosion = Instantiate(manager.basicImpactExplosionVFX, hit.point, Quaternion.identity);
            enemyExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(1f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
            Destroy(this.gameObject);
            yield break;
        }

        // Surface impact Ammo Pickup & VFX
        Vector2 reflect = Vector2.Reflect(dir, hit.normal);
        GameObject surfaceAmmoPickup = Instantiate(manager.basicAmmoPickup, hit.point + new Vector2(reflect.x * 0.1f, reflect.y * 0.1f), Quaternion.identity);
        surfaceAmmoPickup.GetComponent<PoliceChiefAmmoPickup>().InitializeAmmoPickup(manager, reflect * 10f);
        GameObject surfaceExplosion = Instantiate(manager.basicImpactExplosionVFX, hit.point, Quaternion.identity);
        surfaceExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(0.5f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
        Destroy(this.gameObject);
    }
}
