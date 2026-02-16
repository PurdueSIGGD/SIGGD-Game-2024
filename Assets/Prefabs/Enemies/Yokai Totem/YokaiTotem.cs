using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class YokaiTotem : MonoBehaviour
{

    Transform player;
    float range;
    [SerializeField] private DamageContext zapDamageContext;
    [SerializeField] bool debuff;
    float multiplier;
    float zapDamage;
    StatManager stats;
    [SerializeField] GameObject rangeVisual;

    [SerializeField] private GameObject zapEmitter;
    [SerializeField] private ParticleSystem zapParticles;

    private bool onCooldown = false;



    void Start()
    {
        stats = GetComponent<StatManager>();
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        player = PlayerID.instance.transform;
        range = stats.ComputeValue("RANGE");
        multiplier = stats.ComputeValue("DAMAGE_MULTIPLIER");
        zapDamage = stats.ComputeValue("ZAP_DAMAGE");
        //GameplayEventHolder.OnDeath += TotemDeath;

        float rangeVisualScale = (range * 2f) / transform.localScale.x;
        rangeVisual.transform.localScale = new Vector3(rangeVisualScale, rangeVisualScale, 1f);
    }

    void Update()
    {
        if (!player)
        {
            return;
        }

        // look for player
        Vector2 dir = transform.TransformDirection(Vector2.right);
        dir = player.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, range, LayerMask.GetMask("Player"));
        Debug.DrawRay(transform.position, dir.normalized * range, Color.red);

        // VFX Setup
        float dist = Mathf.Min(Vector3.Distance(transform.position, hit.point), range);
        ParticleSystem.MainModule mainModule = zapParticles.main;
        mainModule.startLifetime = dist / (range * 10f);
        Vector3 zapDir = player.position - transform.position;
        float angle = Mathf.Atan2(zapDir.y, zapDir.x) * Mathf.Rad2Deg;
        zapEmitter.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // apply debuff if within LOS and debuff not already on
        if (hit && hit.collider.gameObject.CompareTag("Player"))
        {
            if (!debuff)
            {
                //GameplayEventHolder.OnDamageFilter.Add(DamageMultiplier);
                debuff = true;
            }
        }
        else
        {
            //GameplayEventHolder.OnDamageFilter.Remove(DamageMultiplier);
            debuff = false;
        }
    }

    /*
    // remove debuff when dead
    void TotemDeath(DamageContext context)
    {
        GameplayEventHolder.OnDamageFilter.Remove(DamageMultiplier);
    }
    */

    void DamageMultiplier(DamageContext context)
    {
        if (context.victim.CompareTag("Player") && debuff && !onCooldown && context.damage > 0f &&
            !(context.damageTypes.Contains(DamageType.STATUS) || context.damageTypes.Contains(DamageType.ENVIRONMENTAL)))
        {
            StartCoroutine(DamageCoroutine());
            //context.damage *= multiplier;
            /*
            context.damage += zapDamage;
            Vector3 dir = player.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            zapEmitter.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            zapParticles.Play();
            */
        }
    }

    private IEnumerator DamageCoroutine()
    {
        onCooldown = true;
        AudioManager.Instance.SFXBranch.PlaySFXTrack("TotemWindup");
        yield return new WaitForSeconds(stats.ComputeValue("ZAP_DELAY"));
        if (!debuff)
        {
            onCooldown = false;
            yield break;
        }

        zapDamageContext.damage = zapDamage;
        zapParticles.Play();
        int zapCount = Mathf.FloorToInt(stats.ComputeValue("ZAP_COUNT"));
        for (int i = 0; i < zapCount; i++)
        {
            AudioManager.Instance.SFXBranch.PlaySFXTrack("TotemZap");
            if (debuff) player.GetComponent<Health>().Damage(zapDamageContext, gameObject);
            yield return new WaitForSeconds(stats.ComputeValue("ZAP_INTERVAL"));
        }

        yield return new WaitForSeconds(stats.ComputeValue("ZAP_COOLDOWN") - stats.ComputeValue("ZAP_INTERVAL"));
        onCooldown = false;
    }

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += DamageMultiplier;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= DamageMultiplier;
    }
}