using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyHealth : MonoBehaviour
{
    /// <summary>
    /// The enemy's starting health.
    /// </summary>
    public int maxHealth;

    /// <summary>
    /// Percent of damage to be ignored.
    /// A value of 0 means all damage is taken.
    /// A value of 1 means no damage is taken.
    /// </summary>
    public float damageReduction;

    /// <summary>
    /// The enemy's current health.
    /// </summary>
    public int health;

    void Start()
    {
        health = maxHealth;

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
    public void TakeDamage(int damage)
    {
        Assert.IsTrue(damage >= 0, "Negative damage is not supported");

        damage = Mathf.RoundToInt(damage * (1 - damageReduction));
        health -= damage;
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
