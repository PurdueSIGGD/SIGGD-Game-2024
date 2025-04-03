using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class KingBasic : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;

    [HideInInspector] public KingManager manager;
    [HideInInspector] public bool isShielding;

    private GameObject shieldCircle;

    // Start is called before the first frame update
    void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        isShielding = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartHeavyChargeUp()
    {
        if (manager.currentShieldHealth <= 0f || manager.getBasicCooldown() > 0f)
        {
            GetComponent<PlayerStateMachine>().EnableTrigger("OPT");
            return;
        }

        // Set flags and inits
        isShielding = true;

        // VFX
        shieldCircle = Instantiate(manager.shieldCircleVFX, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        shieldCircle.GetComponent<CircleAreaHandler>().playCircleStart(1.4f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);

        // Start invincibility
        GameplayEventHolder.OnDamageFilter.Add(invincibilityFilter);
    }

    public void StopHeavyChargeUp()
    {
        isShielding = false;
        float maxCooldown = manager.GetStats().ComputeValue("Shield Max Health") / manager.GetStats().ComputeValue("Shield Health Regeneration Rate");
        float cooldownMultiplier = (manager.GetStats().ComputeValue("Shield Max Health") - manager.currentShieldHealth - manager.GetStats().ComputeValue("Shield Health Cooldown Threshold"))
                                   / manager.GetStats().ComputeValue("Shield Max Health");
        manager.setBasicCooldown(maxCooldown * cooldownMultiplier);

        // VFX
        if (shieldCircle != null) shieldCircle.GetComponent<CircleAreaHandler>().playCircleEnd();

        // End invincibility
        GameplayEventHolder.OnDamageFilter.Remove(invincibilityFilter);
    }

    public void invincibilityFilter(ref DamageContext context)
    {
        if (context.victim.tag != "Player") return;

        // Shield Damage Absorb
        manager.currentShieldHealth = Mathf.Max(manager.currentShieldHealth - context.damage, 0f);
        context.damage = 0f;

        // Shield Destroyed
        if (manager.currentShieldHealth <= 0f)
        {
            // VFX
            GameObject shieldExplosion = Instantiate(manager.shieldExplosionVFX, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            shieldExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(3f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);

            // Cancel Shield
            GetComponent<PlayerStateMachine>().EnableTrigger("OPT");
        }
    }
}
