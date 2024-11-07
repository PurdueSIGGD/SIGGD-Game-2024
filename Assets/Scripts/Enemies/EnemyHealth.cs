using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyHealth : MonoBehaviour, IDamageable
{

    /// <summary>
    /// Percent of damage to be ignored.
    /// A value of 0 means all damage is taken.
    /// A value of 1 means no damage is taken.
    /// </summary>
    public float damageReduction;

    /// <summary>
    /// The enemy's current health.
    /// </summary>
    private int health;

    private Stats stats;

    void Start()
    {
        health = stats.GetStatIndex("Health");

        if (damageReduction < 0 || damageReduction > 1)
        {
            Debug.LogWarning($"Enemy {name} has an invalid damage reduction {damageReduction}, must be between 0 and 1");
            damageReduction = Mathf.Clamp01(damageReduction);
        }
    }

    /// <summary>
    /// Take damage, with damage reduction taken into effect.
    /// </summary>
    /// <param ghostName="damage">Amount of damage</param>
    public void TakeDamage(float damage)
    {
        Assert.IsTrue(damage >= 0, "Negative damage is not supported");

        damage = Mathf.RoundToInt(damage * (1 - damageReduction));
        health -= (int)damage;
        print("Enemy took damage");
        if (health <= 0)
        {
            Kill();
        }
    }

    /// <summary>
    /// Immediately kill the enemy.
    /// </summary>
    public void Kill()
    {
        Destroy(this.gameObject);
    }
}
