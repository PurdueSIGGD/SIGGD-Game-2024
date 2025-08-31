using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class RadientWellEffect : MonoBehaviour
{
    [SerializeField] float duration;

    [Header("Outgoing Damage Percentage Modifiers")]
    [SerializeField] float damageBuff = 10;

    [Header("Incoming Damage Percentage Modifiers")]
    [SerializeField] float damageResistance = 10;

    [Header("Speed Percentage Modifiers")]
    [SerializeField] int maxSpeedMod = 10;
    [SerializeField] int runningAccelMod = 10;
    [SerializeField] int ariborneAccelMod = 10;

    [SerializeField]
    List<int> values = new List<int>
    {
        0, 7, 14, 21, 28
    };

    [SerializeField] private float lingeringBuffDuration = 3f;
    private float lingeringTimer = 0f;
    private bool isLingering = false;

    private int skillPts;
    private bool buffActive;
    private GameObject player;
    private StatManager stats;

    void Start()
    {
        player = PlayerID.instance.gameObject;
        stats = player.GetComponent<StatManager>();
    }

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageFilter.Add(CosmeticDamageBuff);
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageFilter.Remove(CosmeticDamageBuff);
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

        if (isLingering)
        {
            lingeringTimer -= Time.deltaTime;
            if (lingeringTimer <= 0f)
            {
                lingeringTimer = 0f;
                isLingering = false;
                if (buffActive) RemoveBuff();
            }
        }
    }

    public void Init(int skillPts)
    {
        this.skillPts = skillPts;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if it's actually the player in the well, and not a clone
        if (collision.gameObject == player)
        {
            if (!buffActive)
            {
                ApplyBuff();
            }
            else if (isLingering)
            {
                isLingering = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (buffActive)
        {
            lingeringTimer = lingeringBuffDuration;
            isLingering = true;
            //RemoveBuff();
        }
    }

    private void ApplyBuff()
    {
        buffActive = true;
        // boost speed
        //playerStat.ModifyStat("Max Running Speed", maxSpeedMod * skillPts);
        //playerStat.ModifyStat("Running Accel.", runningAccelMod * skillPts);
        //playerStat.ModifyStat("Airborne Accel.", runningAccelMod * skillPts);

        // boost damage and dr
        //GameplayEventHolder.OnDamageFilter.Add(DamageBuff);

        stats.ModifyStat("Max Running Speed", values[skillPts]);
        stats.ModifyStat("Running Accel.", values[skillPts]);
        stats.ModifyStat("Max Glide Speed", values[skillPts]);
        stats.ModifyStat("Glide Accel.", values[skillPts]);

        stats.ModifyStat("General Attack Damage Boost", values[skillPts]);

        stats.ModifyStat("Damage Resistance", values[skillPts]);

        // VFX
        PlayerParticles.instance.PlayRadiantWellBuff();
    }

    private void RemoveBuff()
    {
        buffActive = false;
        //playerStat.ModifyStat("Max Running Speed", maxSpeedMod * -skillPts);
        //playerStat.ModifyStat("Running Accel.", runningAccelMod * -skillPts);
        //playerStat.ModifyStat("Airborne Accel.", runningAccelMod * -skillPts);

        //GameplayEventHolder.OnDamageFilter.Remove(DamageBuff);

        stats.ModifyStat("Max Running Speed", -values[skillPts]);
        stats.ModifyStat("Running Accel.", -values[skillPts]);
        stats.ModifyStat("Max Glide Speed", -values[skillPts]);
        stats.ModifyStat("Glide Accel.", -values[skillPts]);

        stats.ModifyStat("General Attack Damage Boost", -values[skillPts]);

        stats.ModifyStat("Damage Resistance", -values[skillPts]);

        // VFX
        PlayerParticles.instance.StopRadiantWellBuff();
    }

    /*
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
    */

    private void CosmeticDamageBuff(ref DamageContext context)
    {
        if (context.attacker.CompareTag("Player") && buffActive)
        {
            context.ghostID = GhostID.KING_AEGIS;
        }
    }
}
