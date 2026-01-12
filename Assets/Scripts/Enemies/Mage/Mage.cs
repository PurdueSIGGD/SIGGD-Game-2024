using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy AI for Mage.
/// </summary>
public class Mage : EnemyStateManager
{

    private GameObject lightningObject;
    // reference to the MageLightningAttack GameObject itself
    private MageLightningAttack lightningScript;

    [Header("Lightning Attack")]

    //[SerializeField] private float lightningDamage = 25;
    [SerializeField] private DamageContext lightningDamage;
    [SerializeField] private float lightningRadius;
    // the size of the ACTUAL lightning attack
    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] GameObject chargeTriggerBox;

    [SerializeField] float followTimeSec = 0.5f;
    [SerializeField] float warningTimeSec = 1f;
    [SerializeField] float lightningTimeSec = 0.33f;
    [SerializeField] bool async; // if true, lightning attack sequence is decoupled from mage casting animation events



    [Header("Basic Projectile")]
    [SerializeField] private GameObject projectile;
    private Vector3 throwPosition = Vector3.zero;

    public void StartBasic()
    {
        throwPosition = PlayerID.instance.transform.position;
        if (!IsCurrentTargetPlayer())
        {
            throwPosition = GetCurrentTarget().transform.position;
        }
    }

    public void FireProjectile()
    {
        Instantiate(projectile, (transform.position + (0.5f * Vector3.right)), transform.rotation).GetComponent<EnemyProjectile>().Init(gameObject, throwPosition);
    }



    private void OnEnable()
    {
        GameplayEventHolder.OnEntityStunned += OnMageStunned;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnEntityStunned -= OnMageStunned;
    }

    public void Update()
    {
        // manually flip the mage to face the target
        GameObject target = player.gameObject;
        if (!IsCurrentTargetPlayer())
        {
            target = GetCurrentTarget();
        }
        if (target != null && (lightningObject == null || lightningScript.IsFollowing()))
        {
            if (target.transform.position.x - transform.position.x < 0)
            {
                Flip(false);
            }
            else
            {
                Flip(true);
            }
        }
    }

    public void StartCharge()
    {
        GameObject target = player.gameObject;
        if (!IsCurrentTargetPlayer())
        {
            target = GetCurrentTarget();
        }

        lightningObject = Instantiate(lightningPrefab, target.transform.position, Quaternion.identity);
        lightningScript = lightningObject.GetComponent<MageLightningAttack>();

        lightningDamage.damage = stats.ComputeValue("Damage");
        lightningScript.Initialize(target, lightningRadius, lightningDamage, gameObject);
        if (async)
        {
            lightningScript.StartIndependentSequence(followTimeSec, warningTimeSec, lightningTimeSec);
        }
    }

    public void StopFollow()
    {
        if (async) return;
        lightningScript.StopFollow();
    }

    public void ActivateLightning()
    {
        if (async) return;
        lightningScript.LightningPhase();
    }

    public void EndLightning()
    {
        if (async) return;

        lightningScript.Fizzle();
        lightningObject = null;
        lightningScript = null;
    }

    /*
    public override bool HasLineOfSight(bool tracking)
    {
        // override L.O.S. calculation to be really super generous to the mage rather than require direct L.O.S.
        return Physics2D.OverlapCircle(chargeTriggerBox.transform.position, chargeTriggerBox.transform.lossyScale.x, LayerMask.GetMask("Player")) || base.HasLineOfSight(tracking);
    }
    */

    // Draws the Mage's attack range
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, chargeTriggerBox.transform.lossyScale.x);
    }
    void OnDestroy()
    {
        if (lightningScript && !async)
        {
            lightningScript.MageDeathHandler();
        }
    }

    public void OnMageStunned(GameObject stunnedEntity)
    {
        if (stunnedEntity != gameObject) return;
        if (lightningScript && !async)
        {
            lightningScript.MageDeathHandler();
        }
    }
}
