using System.Collections;
using UnityEngine;

public class ClericBubbleShieldScript : MonoBehaviour
{

    [SerializeField] private bool isElite;
    [SerializeField] private float eliteDuration;
    Health health;
    GameObject parentEnemy;

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
        if (isElite) StartCoroutine(DurationTimer(eliteDuration));
    }

    public void SetParentEnemy(GameObject enemy)
    {
        parentEnemy = enemy;
    }

    public void TransferDamageToBubbleDamageFilter(ref DamageContext damage)
    {
        if (damage.victim != parentEnemy)
        {
            return;
        }
        DamageContext transferDamage = CreateTransferDamageContext(damage);
        health.Damage(transferDamage, transferDamage.attacker);
        damage.damage = 0; // negate damage to parent enemy
        print("ABSORBED!");
        return;
    }

    private DamageContext CreateTransferDamageContext(DamageContext damage)
    {
        DamageContext newContext = damage;
        newContext.victim = this.gameObject;
        newContext.extraContext += "[transfered from " + damage.victim.name + "]";
        return newContext;
    }



    private IEnumerator DurationTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
