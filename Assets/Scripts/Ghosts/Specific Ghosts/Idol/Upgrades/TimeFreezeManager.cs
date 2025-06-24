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
    private DamageContext killContext;

    struct FrozenEntity
    {
        public Rigidbody2D rb;
        public RigidbodyType2D bodyType;
        public Vector2 velocity;

        public bool delayedDeath;
        public bool delayedStun;

        public float storedMomentumStrength;
        public Vector2 storedMomentumAngle;

        public FrozenEntity(Rigidbody2D rb, RigidbodyType2D bodyType, Vector2 velocity)
        {
            this.rb = rb;
            this.bodyType = bodyType;
            this.velocity = velocity;

            delayedDeath = false;
            delayedStun = false;

            storedMomentumStrength = 0;
            storedMomentumAngle = Vector2.zero;
        }
    }

    public void FreezeTime(float duration)
    {
        if (isActive) { return; }
        isActive = true;
        // Grab every freezeable entity, and freeze them
        foreach (Rigidbody2D rb in GameObject.FindObjectsOfType<Rigidbody2D>())
        {
            if (rb.bodyType == RigidbodyType2D.Static) { continue; }

            GameObject entity = rb.gameObject;
            
            if (entity.CompareTag("Player")) { continue; }

            Vector2 velocity = rb.velocity;
            RigidbodyType2D bodyType = rb.bodyType;
            frozenEntities.Add(entity, new FrozenEntity(rb, bodyType, velocity));

            rb.bodyType = RigidbodyType2D.Static;

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
            if (obj == null) { continue; } // projectiles we collided into is no longer accessible
            FrozenEntity entity = frozenEntities[obj];
            entity.rb.bodyType = entity.bodyType;
            entity.rb.velocity = entity.velocity;
            // apply stored forces here

            if (entity.delayedStun)
            {
                killContext.victim = obj;
                obj.GetComponent<StunMeter>().Damage(killContext);
            }
            if (entity.delayedDeath)
            {
                killContext.victim = obj;
                obj.GetComponent<Health>().Kill(killContext);
            }
        }
        frozenEntities.Clear();
    }

    public static bool GetIsActive() { return isActive; }

    void PauseDeathFilter(ref DamageContext context)
    {
        if (!GetIsActive()) return;
        if (frozenEntities.ContainsKey(context.victim))
        {
            FrozenEntity entity = frozenEntities[context.victim];
            Health health = context.victim.GetComponent<Health>();
            StunMeter stun = context.victim.GetComponent<StunMeter>();

            // pause death
            if (context.damage >= health.currentHealth)
            {
                context.damage = health.currentHealth - 0.001f;
                entity.delayedDeath = true;
                frozenEntities[context.victim] = entity;
                Debug.Log(entity.rb);
            }

            // pause stun meter build up
            if (stun.currentStun + stun.ComputeStunBuildUp(context.damageStrength) >= stun.maxStun)
            {
                context.damageStrength = DamageStrength.MEAGER;
                stun.currentStun = stun.maxStun - 0.001f;
                entity.delayedStun = true;
                frozenEntities[context.victim] = entity;
            }
        }
    }

    void Start()
    {
        frozenEntities = new Dictionary<GameObject, FrozenEntity>();
        playerRef = PlayerID.instance.GameObject();
        GameplayEventHolder.OnDamageFilter.Add(PauseDeathFilter);
        
        killContext = new DamageContext();
        killContext.attacker = playerRef;
        killContext.actionID = ActionID.MISCELLANEOUS;
        killContext.actionTypes = new List<ActionType>() { ActionType.MISCELLANEOUS };
        killContext.damageStrength = DamageStrength.LIGHT;
        killContext.damageTypes = new List<DamageType>() { DamageType.STATUS };
        killContext.invokingScript = this;
    }
}
