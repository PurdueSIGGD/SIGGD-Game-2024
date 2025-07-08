using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadientWellEffect : MonoBehaviour
{
    [SerializeField] float duration;

    [Header("Outgoing Damage Percentage Modifiers")]
    [SerializeField] int damageBuff = 10;

    [Header("Incoming Damage Percentage Modifiers")]
    [SerializeField] float damageResistance = 10;

    [Header("Speed Percentage Modifiers")]
    [SerializeField] int maxSpeedMod = 10;
    [SerializeField] int runningAccelMod = 10;
    [SerializeField] int ariborneAccelMod = 10;

    private int skillPts;
    private bool buffActive;
    private GameObject player;
    private StatManager playerStat;

    void Start()
    {
        player = PlayerID.instance.gameObject;
        playerStat = player.GetComponent<StatManager>();
    }

    void Update()
    {
        if (duration <= 0)
        {
            if (buffActive)
            {
                RemoveBuff();
            }
            Destroy(gameObject);
        }
        duration -= Time.deltaTime;
    }

    public void Init(int skillPts)
    {
        this.skillPts = skillPts;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if it's actually the player in the well, and not a clone
        if (collision.gameObject == player && !buffActive)
        {
            ApplyBuff();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (buffActive)
        {
            RemoveBuff();
        }
    }

    private void ApplyBuff()
    {
        buffActive = true;
        // boost speed
        playerStat.ModifyStat("Max Running Speed", maxSpeedMod * skillPts);
        playerStat.ModifyStat("Running Accel.", runningAccelMod * skillPts);
        playerStat.ModifyStat("Airborne Accel.", runningAccelMod * skillPts);

        // boost damage and dr
        GameplayEventHolder.OnDamageFilter.Add(DamageBuff);
    }

    private void RemoveBuff()
    {
        buffActive = false;
        playerStat.ModifyStat("Max Running Speed", maxSpeedMod * -skillPts);
        playerStat.ModifyStat("Running Accel.", runningAccelMod * -skillPts);
        playerStat.ModifyStat("Airborne Accel.", runningAccelMod * -skillPts);

        GameplayEventHolder.OnDamageFilter.Remove(DamageBuff);
    }

    private void DamageBuff(ref DamageContext context)
    {
        // decrease the damage the player takes
        if (context.victim.CompareTag("Player"))
        {
            context.damage *= 1 - damageResistance * skillPts / 100;
        }
        // increase the damage the player deals
        else if (context.attacker.CompareTag("Player"))
        {
            context.damage *= 1 + damageBuff * skillPts / 100;
        }
    }
}
