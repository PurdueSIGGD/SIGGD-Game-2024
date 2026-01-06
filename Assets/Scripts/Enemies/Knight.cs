using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : EnemyStateManager
{
    [Header("Sword Attack")]
    [SerializeField] protected Transform swordTrigger;
    [SerializeField] protected DamageContext swordDamage;
    [SerializeField] float damageVal;
    [SerializeField] GameObject swingVisual;

    [Header("Basic Attack")]
    [SerializeField] protected Transform basicTrigger;
    [SerializeField] protected DamageContext basicDamage;
    [SerializeField] float basicDamageVal;


    protected override void Start()
    {
        base.Start();
        swordDamage.damage = damageVal;
        basicDamage.damage = basicDamageVal;
    }

    private void OnEnable()
    {
        GameplayEventHolder.OnEntityStunned += OnKnightStunned;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnEntityStunned -= OnKnightStunned;
    }

    protected void OnStartSlash()
    {
        swingVisual.SetActive(true);
    }

    // Check for collision in swing range to deal damage
    protected void OnSlashEvent()
    {
        GenerateDamageFrame(swordTrigger.position, swordTrigger.lossyScale.x, swordTrigger.lossyScale.y, swordDamage, gameObject);
        swingVisual.SetActive(true);
    }

    protected void OnBasicEvent()
    {
        GenerateDamageFrame(basicTrigger.position, basicTrigger.lossyScale.x, basicTrigger.lossyScale.y, basicDamage, gameObject);
        swingVisual.SetActive(true);
    }

    protected void OnEndSlash()
    {
        swingVisual.SetActive(false);
    }

    private void OnKnightStunned(GameObject stunnedEntity)
    {
        if (stunnedEntity == gameObject)
        {
            swingVisual.SetActive(false);
        }
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(swordTrigger.position, swordTrigger.lossyScale);
    }
}
