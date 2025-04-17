using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PoliceChiefRailgunShot : MonoBehaviour
{
    private PoliceChiefManager manager;
    private float travelSpeed = 300f;
    private List<GameObject> damagedEnemies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fireRailgunShot(PoliceChiefManager manager, Vector2 pos, Vector2 dir)
    {
        this.manager = manager;
        StartCoroutine(railgunShotCoroutine(pos, dir));
    }

    private IEnumerator railgunShotCoroutine(Vector2 pos, Vector2 dir)
    {
        damagedEnemies = new List<GameObject>();
        float remainingDistance = manager.GetStats().ComputeValue("Special Travel Distance");

        // Loop for each ricocheting shot
        for (int i = 0; i <= manager.GetStats().ComputeValue("Special Ricochet Count"); i++)
        {
            if (i > 0) yield return new WaitForSeconds(0.04f);

            // Calculate shot vector
            RaycastHit2D hit = Physics2D.Raycast(pos, dir, remainingDistance, LayerMask.GetMask("Ground"));
            Vector2 hitPoint = (hit) ? hit.point : pos + (dir * remainingDistance);
            RaycastHit2D[] enemyHits = Physics2D.RaycastAll(pos, (hitPoint - pos), Vector2.Distance(pos, hitPoint), LayerMask.GetMask("Enemy"));
            remainingDistance -= Vector2.Distance(pos, hitPoint);

            // VFX
            Debug.DrawLine(pos, hitPoint, Color.red, 5.0f);
            CameraShake.instance.Shake(0.35f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
            GameObject railgunTracer = Instantiate(manager.specialTracerVFX, Vector3.zero, Quaternion.identity);
            railgunTracer.GetComponent<RaycastTracerHandler>().playTracer(pos, hitPoint, travelSpeed, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);

            // Affect enemies
            foreach (RaycastHit2D enemyHit in enemyHits)
            {
                StartCoroutine(handleAffectedEnemy(enemyHit, pos));
            }

            // Wait for travel speed
            yield return new WaitForSeconds(Vector2.Distance(pos, hitPoint) / travelSpeed);
            if (!hit)
            {
                break;
            }

            // Surface impact VFX
            GameObject surfaceExplosion = Instantiate(manager.specialImpactExplosionVFX, hit.point, Quaternion.identity);
            surfaceExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(2f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);

            // Calculate shot aiming vector for next ricochet
            if (i == manager.GetStats().ComputeValue("Special Ricochet Count")) break;
            float hitAngle = Vector2.Angle((pos - hit.point), hit.normal);
            if (hitAngle < manager.GetStats().ComputeValue("Special Ricochet Minimum Normal Angle")) break;
            Vector2 reflect = Vector2.Reflect(dir, hit.normal);
            pos = hit.point + new Vector2(reflect.x * 0.1f, reflect.y * 0.1f);
            dir = reflect.normalized;
        }
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    private IEnumerator handleAffectedEnemy(RaycastHit2D enemyHit, Vector2 pos)
    {
        yield return new WaitForSeconds(Vector2.Distance(pos, enemyHit.point) / travelSpeed);

        // Damage enemies that have not yet been hit
        if (damagedEnemies.Contains(enemyHit.transform.gameObject)) yield break;
        damagedEnemies.Add(enemyHit.transform.gameObject);
        enemyHit.transform.gameObject.GetComponent<Health>().Damage(manager.specialDamage, PlayerID.instance.gameObject);

        // Enemy impact VFX
        GameObject enemyExplosion = Instantiate(manager.specialImpactExplosionVFX, enemyHit.point, Quaternion.identity);
        enemyExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(3f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
    }
}
