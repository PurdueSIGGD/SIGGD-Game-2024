using UnityEngine;

public class ClericBubbleShieldScript : MonoBehaviour
{
    Health health;

    void Awake()
    {
        GameplayEventHolder.OnDamageFilter.Add(TransferDamageToBubbleDamageFilter);
    }

    void OnDestroy()
    {
        GameplayEventHolder.OnDamageFilter.Remove(TransferDamageToBubbleDamageFilter);
    }

    void Start()
    {
        health = GetComponent<Health>();
    }

    public void TransferDamageToBubbleDamageFilter(ref DamageContext damage)
    {
        if (damage.victim != GetComponentInParent<Health>().gameObject)
        {
            return;
        }
        DamageContext transferDamage = damage; // creates a copy of the damageContext ref
        GameObject attacker = transferDamage.attacker;
        health.Damage(transferDamage, attacker);
        damage.damage = 0; // negate damage to parent enemy
    }
}
