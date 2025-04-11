using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoliceChiefSpecial : MonoBehaviour, ISpecialMove
{
    private bool shouldChangeBack = true;
    private PlayerStateMachine playerStateMachine;
    private Animator camAnim;
    private Camera cam;

    [HideInInspector] public PoliceChiefManager manager;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    void Update()
    {
        if (manager != null)
        {
            if (manager.getSpecialCooldown() > 0)
            {
                playerStateMachine.OnCooldown("c_special");
            }
            else
            {
                playerStateMachine.OffCooldown("c_special");
            }
        }
    }

    public void CheckPullBack()
    {
        if (shouldChangeBack) {
            endSpecial(false);
        }
        shouldChangeBack = true;
    }

    void StartSpecialChargeUp()
    {
        camAnim.SetBool("pullBack", true);
        GetComponent<Move>().PlayerStop();
    }

    void StopSpecialChargeUp()
    {
        CheckPullBack();
    }

    void StartSpecialPrimed()
    {
        shouldChangeBack = false;
    }

    void StopSpecialPrimed()
    {
        CheckPullBack();
    }

    void StartSpecialAttack()
    {
        StartCoroutine(StartSpecialAttackCoroutine());
    }

    private IEnumerator StartSpecialAttackCoroutine()
    {
        // Disable ghost swap and camera pull-in
        shouldChangeBack = false;
        GetComponent<PartyManager>().SetSwappingEnabled(false);
        List<GameObject> damagedEnemies = new List<GameObject>();

        // Calculate initial shot aiming vector
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = transform.position;
        Vector2 dir = (mousePos - pos).normalized;

        // Loop for each ricocheting shot
        for (int i = 0; i <= manager.GetStats().ComputeValue("Special Ricochet Count"); i++)
        {
            if (i > 0) yield return new WaitForSeconds(0.08f);
            CameraShake.instance.Shake(0.35f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));

            // Calculate shot vector
            RaycastHit2D hit = Physics2D.Raycast(pos, dir, manager.GetStats().ComputeValue("Special Travel Distance"), LayerMask.GetMask("Ground"));
            Vector2 hitPoint = (hit) ? hit.point : pos + (dir * manager.GetStats().ComputeValue("Special Travel Distance"));
            RaycastHit2D[] enemyHits = Physics2D.RaycastAll(pos, (hitPoint - pos), Vector2.Distance(pos, hitPoint), LayerMask.GetMask("Enemy"));

            // VFX
            Debug.DrawLine(pos, hitPoint, Color.red, 5.0f);
            GameObject railgunTracer = Instantiate(manager.specialTracerVFX, Vector3.zero, Quaternion.identity);
            railgunTracer.GetComponent<RaycastTracerHandler>().playTracerFade(pos, hitPoint, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);

            // Affect enemies
            foreach(RaycastHit2D enemyHit in enemyHits)
            {
                // Damage enemies that have not yet been hit
                if (damagedEnemies.Contains(enemyHit.transform.gameObject)) continue;
                damagedEnemies.Add(enemyHit.transform.gameObject);
                enemyHit.transform.gameObject.GetComponent<Health>().Damage(manager.specialDamage, gameObject);

                // Enemy impact VFX
                GameObject enemyExplosion = Instantiate(manager.specialImpactExplosionVFX, enemyHit.transform.position, Quaternion.identity);
                enemyExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(3f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
            }

            // Surface impact VFX
            if (hit)
            {
                GameObject enemyExplosion = Instantiate(manager.specialImpactExplosionVFX, hit.point, Quaternion.identity);
                enemyExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(2f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
            }

            // Calculate shot aiming vector for next ricochet
            if (!hit || i == manager.GetStats().ComputeValue("Special Ricochet Count")) break;
            float hitAngle = Vector2.Angle((pos - hit.point), hit.normal);
            if (hitAngle < manager.GetStats().ComputeValue("Special Ricochet Minimum Normal Angle")) break;
            Vector2 reflect = Vector2.Reflect(dir, hit.normal);
            pos = hit.point + new Vector2(reflect.x * 0.1f, reflect.y * 0.1f);
            dir = reflect.normalized;
        }
        // Reenable ghost swap
        GetComponent<PartyManager>().SetSwappingEnabled(true);
    }

    void StopSpecialAttack()
    {
        endSpecial(true);
    }

    /// <summary>
    /// End the special ability if it is active.
    /// </summary>
    /// <param name="startCooldown">If true, the special ability's cooldown will begin when the ability ends.</param>
    public void endSpecial(bool startCooldown)
    {
        camAnim.SetBool("pullBack", false);
        GetComponent<Move>().PlayerGo();
        if (!startCooldown) return;
        playerStateMachine.OnCooldown("c_special");
        manager.startSpecialCooldown();
    }

    public bool GetBool()
    {
        return true;
    }
}
