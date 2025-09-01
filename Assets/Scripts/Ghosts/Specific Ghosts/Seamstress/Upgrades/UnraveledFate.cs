#define DEBUG_LOG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fatebound enemies release a flurry of threads when defeated, damaging all other fatebound enemies.
/// Damage: 15 | 25 | 35 | 45
/// </summary>
public class UnraveledFate : Skill
{

    private SeamstressManager manager;

    [SerializeField]
    private List<float> values = new List<float>
    {
        0, 10, 20, 30, 40
    };

    [SerializeField] private DamageContext damage;

    [SerializeField] private GameObject unraveledFateVFX;
    [SerializeField] private GameObject pulseVFX;
    [SerializeField] private Color colorVFX;

    private SeamstressManager GetManager()
    {
        if (manager == null)
        {
            manager = gameObject.GetComponent<SeamstressManager>();
        }
        return manager;
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    /// <summary>
    /// Damages fatebound enemies, called in seamstress manager when fatebound enemy dies
    /// </summary>
    /// <param name="enemyID"> The instance id of the enemy that was defeated </param>
    public void DamageFateboundEnemies(int enemyID, Vector3 position)
    {
        if (GetPoints() <= 0) { return; }

        if (!GetManager()) { return; }

        StartCoroutine(DamageDelay(enemyID, position));

        //int damageAmount = (GetPoints() - 1) * 10 + 15;
        //int 

        ////float damageAmount = values[GetPoints()];

        //DamageContext damage = new DamageContext();

        ////damage.damage = damageAmount;

        //damage.damageStrength = DamageStrength.MEAGER;
        //damage.damageTypes = new List<DamageType>() { DamageType.PROJECTILE };
        //damage.actionID = ActionID.MISCELLANEOUS;
        //damage.actionTypes = new List<ActionType>() { ActionType.SKILL };

        //manager.DamageLinkedEnemies(enemyID, damage, false);

#if DEBUG_LOG
        //Debug.Log("Unraveled Fate: Damaged fatebound enemies by " + damageAmount);
#endif

    }

    private IEnumerator DamageDelay(int enemyID, Vector3 position)
    {
        GameObject unraveledFate = Instantiate(unraveledFateVFX, position, Quaternion.identity);
        float damageAmount = values[GetPoints()];
        damage.damage = damageAmount;

        yield return new WaitForSeconds(0.5f);

        Destroy(unraveledFate);
        manager.DamageLinkedEnemies(enemyID, damage, false);
        GameObject pulse = Instantiate(pulseVFX, position, Quaternion.identity);
        pulse.GetComponent<RingExplosionHandler>().playRingExplosion(3f, colorVFX);

        AudioManager.Instance.VABranch.PlayVATrack("Yume-Seamstress Unraveled Fate");
    }

    public override void AddPointTrigger()
    {
    }

    public override void ClearPointsTrigger()
    {
    }

    public override void RemovePointTrigger()
    {
    }
}
