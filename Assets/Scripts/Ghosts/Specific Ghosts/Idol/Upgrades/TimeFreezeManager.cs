using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TimeFreezeManager : MonoBehaviour
{
    private static bool isActive;
    Dictionary<GameObject, FrozenEntity> frozenEntities;

    private GameObject playerRef;

    struct FrozenEntity
    {
        public Rigidbody2D rb;
        public Vector2 velocity;
        public Health health;
        public StunMeter stun;

        public float storedMomentumStrength;
        public Vector2 storedMomentumAngle;

        public FrozenEntity(Rigidbody2D rb, Vector2 velocity, Health health, StunMeter stun)
        {
            this.rb = rb;
            this.velocity = velocity;
            this.health = health;
            this.stun = stun;
            storedMomentumStrength = 0;
            storedMomentumAngle = Vector2.zero;
        }
    }

    public void FreezeTime(float duration)
    {
        isActive = true;
        // Grab every freezeable entity, and freeze them
        foreach (Rigidbody2D rb in GameObject.FindObjectsOfType<Rigidbody2D>())
        {
            if (rb.bodyType == RigidbodyType2D.Static) { continue; }

            GameObject entity = rb.gameObject;
            
            if (entity.CompareTag("Player")) { continue; }

            Vector2 velocity = rb.velocity;
            Health health = entity.GetComponent<Health>();
            StunMeter stun = entity.GetComponent<StunMeter>();

            rb.bodyType = RigidbodyType2D.Static;

            frozenEntities.Add(entity, new FrozenEntity(rb, velocity, health, stun));

            if (entity.CompareTag("Enemy"))
            {
                EnemyStateManager esm = entity.GetComponent<EnemyStateManager>();
                esm.Stun(new DamageContext(), duration);
            }
        }
    }

    public void UnFreezeTime()
    {
        isActive = false;
        foreach (GameObject obj in frozenEntities.Keys)
        {
            FrozenEntity entity = frozenEntities[obj];
            entity.rb.bodyType = RigidbodyType2D.Dynamic;
            entity.rb.velocity = entity.velocity;


            if (entity.health != null && entity.stun != null)
            {
                DamageContext context = new DamageContext();
                entity.health.currentHealth -= 0.001f;
                entity.stun.currentStun -= 0.001f;
                entity.health.Damage(context, playerRef);
                entity.stun.Damage(context, playerRef);
            }
            // apply stored forces here
        }
        frozenEntities.Clear();
    }

    public static bool GetIsActive() { return isActive; }

    void PauseDeathFilter(ref DamageContext context)
    {
        if (!GetIsActive()) return;
        if (frozenEntities.ContainsKey(context.victim))
        {
            FrozenEntity victim = frozenEntities[context.victim];
            context.damage = context.damage >= victim.health.currentHealth ? victim.health.currentHealth - 0.001f : context.damage;

            // pause stun meter build up
            if (victim.stun.currentStun + victim.stun.ComputeStunBuildUp(context.damageStrength) >= victim.stun.maxStun)
            {
                context.damageStrength = DamageStrength.MEAGER;
                victim.stun.currentStun = victim.stun.maxStun - 0.001f;
            }
        }
    }

    void Start()
    {
        frozenEntities = new Dictionary<GameObject, FrozenEntity>();
        playerRef = PlayerID.instance.GameObject();
        GameplayEventHolder.OnDamageFilter.Add(PauseDeathFilter);
    }
}
