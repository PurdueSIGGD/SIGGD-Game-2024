using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy AI for Riot Police
/// </summary>
public class RiotPolice : EnemyStateManager
{
    [Header("Baton Attack")]
    [SerializeField] protected Transform batonTrigger;
    [SerializeField] protected DamageContext batonDamage;
    [SerializeField] GameObject batonVisual;

    protected override void Start()
    {
        base.Start();
        batonDamage.damage = stats.ComputeValue("Damage");
    }

    // Check for collision in swing range to deal damage
    protected void OnBatonEvent()
    {
        GenerateDamageFrame(batonTrigger.position, batonTrigger.lossyScale.x, batonTrigger.lossyScale.y, batonDamage, gameObject);
        batonVisual.SetActive(true);
    }

    protected void OnBatonEnd()
    {
        batonVisual.SetActive(false);
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(batonTrigger.position, batonTrigger.lossyScale);
    }
}
