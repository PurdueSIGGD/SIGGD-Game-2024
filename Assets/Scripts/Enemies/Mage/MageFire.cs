using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy AI for Mage.
/// </summary>
public class MageFire : EnemyStateManager
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] GameObject lineOfSightTriggerBox;
    [Header("FireBall Damage")]
    [SerializeField] private DamageContext fireDamageImpact;
    [SerializeField] float impactDamage;
    [SerializeField] private DamageContext fireDamageTick;
    [SerializeField] float tickDamage;
    [Header("FireBall Settings")]
    [SerializeField] float baseSpeed;
    [SerializeField] float trackIntensity;
    [SerializeField] float damageTickIntervalSec;
    [SerializeField] float damageTickDurationSec;

    new void Awake()
    {
        base.Awake();
        fireDamageImpact.damage = impactDamage;
        fireDamageTick.damage = tickDamage;
    }

    public void Update()
    {
        // manually flip the mage to face the player
        if (player != null)
        {
            if (player.position.x - transform.position.x < 0)
            {
                Flip(false);
            }
            else
            {
                Flip(true);
            }
        }
    }
    public void FireFireBall()
    {
        MageFireBallAttack ball = Instantiate(fireballPrefab, transform.position, Quaternion.identity).GetComponent<MageFireBallAttack>();
        ball.Initialize(player.gameObject, this.gameObject, fireDamageImpact, fireDamageTick, baseSpeed,
            trackIntensity, damageTickIntervalSec, damageTickDurationSec);
    }
    public override bool HasLineOfSight(bool tracking)
    {
        // override L.O.S. calculation to be really super generous to the mage rather than require direct L.O.S.
        return Physics2D.OverlapCircle(lineOfSightTriggerBox.transform.position, lineOfSightTriggerBox.transform.lossyScale.x, LayerMask.GetMask("Player")) || base.HasLineOfSight(tracking);
    }

    // Draws the Mage's attack range
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, lineOfSightTriggerBox.transform.lossyScale.x);
    }
}
