using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy AI for Mage.
/// </summary>
public class Mage : EnemyStateManager
{

    private GameObject lightningObject;   // reference to the MageLightningAttack GameObject itself
    private MageLightningAttack lightningScript;

    [Header("Lightning Attack")]

    //[SerializeField] private float lightningDamage = 25;
    [SerializeField] private DamageContext lightningDamage;
    [SerializeField] private float lightningRadius;   // the size of the ACTUAL lightning attack
    [SerializeField] private float lightningChargeDuration;  // how long in seconds the lightning is charging before damage is applied

    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] GameObject chargeTriggerBox;
    [SerializeField] bool isCharging;

    public void StartCharge()
    {
        lightningObject = Instantiate(lightningPrefab, player.position, Quaternion.identity);
        lightningScript = lightningObject.GetComponent<MageLightningAttack>();

        lightningDamage.damage = stats.ComputeValue("Damage");
        lightningScript.Initialize(player.position, lightningRadius, lightningDamage, lightningChargeDuration, gameObject);

        isCharging = true;
    }

    public void ActivateLightning()
    {
        lightningScript.LightningPhase();
    }

    public void EndLightning()
    {
        isCharging = false;
        lightningScript.Fizzle();
        lightningObject = null;
        lightningScript = null;
    }

    void Update()
    {
        // manually flip the mage to face the player while charging attack
        if (isCharging)
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

    public override bool HasLineOfSight(bool tracking)
    {
        // override L.O.S. calculation to be really super generous to the mage rather than require direct L.O.S.
        return Physics2D.OverlapCircle(chargeTriggerBox.transform.position, chargeTriggerBox.transform.lossyScale.x, LayerMask.GetMask("Player"));
    }

    // Draws the Mage's attack range
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, chargeTriggerBox.transform.lossyScale.x);
    }
}
