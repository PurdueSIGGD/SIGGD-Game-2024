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

    [Header("Lightning Attack")]

    //[SerializeField] private float lightningDamage = 25;
    [SerializeField] private DamageContext lightningDamage;
    [SerializeField] private float lightningRadius;   // the size of the ACTUAL lightning attack
    [SerializeField] private float lightningChargeDuration;  // how long in seconds the lightning is charging before damage is applied

    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] GameObject chargeTriggerBox;

    // Starting to cast lightning
    public void Attack()
    {
        // spawn a lightning prefab
        lightningObject = Instantiate(lightningPrefab, player.position, Quaternion.identity);
        MageLightningAttack attack = lightningObject.GetComponent<MageLightningAttack>();

        lightningDamage.damage = stats.ComputeValue("Damage");
        attack.Initialize(player.position, lightningRadius, lightningDamage, lightningChargeDuration, gameObject);
        attack.StartCharging();
    }

    void Update()
    {

    }


    // Draws the Mage's attack range
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        //Gizmos.DrawWireSphere(transform.position, chargeTriggerBox.transform.lossyScale.x);
    }
}
