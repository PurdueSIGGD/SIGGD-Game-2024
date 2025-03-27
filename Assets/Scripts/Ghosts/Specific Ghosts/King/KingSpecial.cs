using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSpecial : MonoBehaviour, ISpecialMove
{
    private PlayerStateMachine playerStateMachine;

    [HideInInspector] public KingManager manager;

    // Start is called before the first frame update
    void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    // Update is called once per frame
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

    public void StartDash()
    {
        StartCoroutine(SpecialAbilityCoroutine());
    }

    private IEnumerator SpecialAbilityCoroutine()
    {
        CameraShake.instance.Shake(0.2f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, manager.GetStats().ComputeValue("Special Explosion Radius"), LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in enemies)
        {
            enemy.gameObject.GetComponent<Health>().Damage(manager.specialDamage, gameObject);
            //enemy.gameObject.GetComponent<EnemyStateManager>().Stun(manager.specialDamage, 0.1f);
            Vector2 enemyDirection = enemy.transform.position - transform.position;
            float knockbackStrength = manager.GetStats().ComputeValue("Special Knockback Strength");
            Vector2 knockbackVector = enemyDirection.normalized * knockbackStrength;
            float bonusUpwardKnockback = manager.GetStats().ComputeValue("Special Minimum Upward Knockback Strength") - knockbackVector.y;
            enemy.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(enemyDirection, knockbackStrength);
            enemy.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(Vector2.up, bonusUpwardKnockback);
        }
        yield return new WaitForSeconds(0.15f);
        playerStateMachine.EnableTrigger("OPT");
        manager.startSpecialCooldown();
    }

    public bool GetBool()
    {
        return true;
    }
}
