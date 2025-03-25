using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class YokaiTotem : MonoBehaviour
{

    Transform player;
    float range;
    [SerializeField] bool debuff;
    float multiplier;
    StatManager stats;
    [SerializeField] GameObject rangeVisual;

    void Start()
    {
        stats = GetComponent<StatManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        range = stats.ComputeValue("RANGE");
        multiplier = stats.ComputeValue("DAMAGE_MULTIPLIER");
        GameplayEventHolder.OnDeath += TotemDeath;

        float rangeDiameter = range * 2;
        rangeVisual.transform.localScale = new Vector3(rangeDiameter, rangeDiameter, 1);
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

        // apply debuff if within LOS and debuff not already on
        if (hit && hit.collider.gameObject.CompareTag("Player"))
        {
            if (!debuff)
            {
                GameplayEventHolder.OnDamageFilter.Add(DamageMultiplier);
                debuff = true;
            }
        }
        else
        {
            GameplayEventHolder.OnDamageFilter.Remove(DamageMultiplier);
            debuff = false;
        }
    }
    // remove debuff when dead
    void TotemDeath(ref DamageContext context)
    {
        GameplayEventHolder.OnDamageFilter.Remove(DamageMultiplier);
    }

    void DamageMultiplier(ref DamageContext context)
    {
        if (context.victim.CompareTag("Player"))
        {
            context.damage = context.damage * multiplier;
        }
    }
}