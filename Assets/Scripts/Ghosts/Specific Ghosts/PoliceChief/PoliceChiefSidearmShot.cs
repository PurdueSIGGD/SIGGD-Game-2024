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
    }

    private IEnumerator sidearmShotCoroutine(Vector2 pos, Vector2 dir)
    {
        // Calculate shot vector
        RaycastHit2D hit = Physics2D.Raycast(pos, dir, manager.GetStats().ComputeValue("Basic Travel Distance"), LayerMask.GetMask("Ground", "Enemy"));
        Vector2 hitPoint = (hit) ? hit.point : pos + (dir * manager.GetStats().ComputeValue("Basic Travel Distance"));

        // VFX
        Debug.DrawLine(pos, hitPoint, Color.red, 5.0f);
        CameraShake.instance.Shake(0.25f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
        GameObject railgunTracer = Instantiate(manager.basicTracerVFX, Vector3.zero, Quaternion.identity);
        railgunTracer.GetComponent<RaycastTracerHandler>().playTracer(pos, hitPoint, travelSpeed, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);

        // Wait for travel speed
        yield return new WaitForSeconds(Vector2.Distance(pos, hitPoint) / travelSpeed);
        if (!hit)
        {
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

        // Surface impact VFX
        GameObject surfaceExplosion = Instantiate(manager.basicImpactExplosionVFX, hit.point, Quaternion.identity);
        surfaceExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(0.5f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
        Destroy(this.gameObject);
    }
}
