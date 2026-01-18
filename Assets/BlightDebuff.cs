using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlightDebuff : MonoBehaviour
{
    [SerializeField] private GameObject empoweredBlightParticles;

    Health health;
    [SerializeField] public DamageContext damageContext;
    [SerializeField] float damage;
    [SerializeField] float interval; // seconds per tick
    [SerializeField] float empoweredInterval;

    private float currentInterval;
    private float timer = 999f;
    public float duration = 999f;

    private SilasManager manager;

    private bool isEmpowered = false;
    private float empoweredDuration = 999f;
    private float timeApplied = 0f;
    //private bool isFlyer = false;


    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += EmpowerDebuff;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= EmpowerDebuff;
    }



    // Start is called before the first frame update
    void Start()
    {
        empoweredBlightParticles.SetActive(false);
        health = gameObject.GetComponentInParent<Health>();
        timer = currentInterval = interval;
        //isFlyer = gameObject.GetComponentInParent<EnemyStateManager>().isFlyer;
    }

    // Update is called once per frame
    void Update()
    {
        // Quicksilver Damage Boost
        timeApplied += Time.deltaTime;
        float quicksilverDamageBoost = Mathf.Lerp(100f, manager.GetStats().ComputeValue("Blight Max Quicksilver Damage Percent"),
                                                  (timeApplied >= manager.GetStats().ComputeValue("Blight Max Quicksilver Time")) ? 1f : (timeApplied / manager.GetStats().ComputeValue("Blight Max Quicksilver Time")));
        //damageContext.damage = damage * (quicksilverDamageBoost / 100f);
        if (isEmpowered)
        {
            damageContext.damage = ((manager.GetStats().ComputeValue("Blight Empowered DPS") - manager.GetStats().ComputeValue("Blight DPS")) + (manager.GetStats().ComputeValue("Blight DPS") * (quicksilverDamageBoost / 100f))) * currentInterval;
        }
        else
        {
            damageContext.damage = damage * (quicksilverDamageBoost / 100f);
        }

        // Empowered timer
        if (isEmpowered)
        {
            empoweredDuration -= Time.deltaTime;
            if (empoweredDuration <= 0f)
            {
                StopEmpoweringDebuff();
            }
        }

        // Blight duration timer
        if (duration <= 0f)
        {
            if (isEmpowered) StopEmpoweringDebuff();
            health = gameObject.GetComponentInParent<Health>();
            if (health != null)
            {
                health.Damage(damageContext, PlayerID.instance.gameObject);
            }

            if (!(gameObject.GetComponentInParent<StatManager>() == null || gameObject.GetComponentInParent<EnemyStateManager>() == null))
                gameObject.GetComponentInParent<StatManager>().ModifyStat((gameObject.GetComponentInParent<EnemyStateManager>().isFlyer) ? "FLIGHT_FORCE" : "Speed", Mathf.FloorToInt(manager.GetStats().ComputeValue("Blight Slow")));
            Destroy(gameObject);
        }
        duration -= Time.deltaTime;

        // Blight damage over time interval
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        health = gameObject.GetComponentInParent<Health>();
        if (health != null)
        {
            health.Damage(damageContext, PlayerID.instance.gameObject);
        }
        timer = currentInterval;
    }



    public void ApplyDebuff(SilasManager manager)
    {
        ApplyDebuff(manager, manager.GetStats().ComputeValue("Blight Duration"));
    }

    public void ApplyDebuff(SilasManager manager, float duration)
    {
        //float interval = 0.2f;
        this.manager = manager;
        damage = manager.GetStats().ComputeValue("Blight DPS") * interval;
        damageContext.damage = damage;
        //this.interval = interval;
        currentInterval = interval;
        this.duration = duration;
        health = gameObject.GetComponentInParent<Health>();
        timer = 0f;

        if (gameObject.GetComponentInParent<StatManager>() == null || gameObject.GetComponentInParent<EnemyStateManager>() == null) return;
        gameObject.GetComponentInParent<StatManager>().ModifyStat((gameObject.GetComponentInParent<EnemyStateManager>().isFlyer) ? "FLIGHT_FORCE" : "Speed", -Mathf.FloorToInt(manager.GetStats().ComputeValue("Blight Slow")));
    }

    public void AddDebuffTime(float time)
    {
        duration += time;
        duration = Mathf.Min(duration, manager.GetStats().ComputeValue("Blight Duration"));
    }

    public void SetDebuffTime(float time)
    {
        duration = Mathf.Max(time, duration);
    }



    public void EmpowerDebuff(DamageContext context)
    {
        if (context.victim.Equals(transform.parent.gameObject) &&
            !context.damageTypes.Contains(DamageType.STATUS) &&
            context.actionID != ActionID.PLAGUE_DOCTOR_SPECIAL)
        {
            if (isEmpowered)
            {
                empoweredDuration = manager.GetStats().ComputeValue("Blight Empowered Duration");
                return;
            }
            isEmpowered = true;
            damage = manager.GetStats().ComputeValue("Blight Empowered DPS") * empoweredInterval;
            currentInterval = empoweredInterval;
            empoweredDuration = manager.GetStats().ComputeValue("Blight Empowered Duration");

            int addedSlow = Mathf.FloorToInt(manager.GetStats().ComputeValue("Blight Empowered Slow")) - Mathf.FloorToInt(manager.GetStats().ComputeValue("Blight Slow"));
            if (!(gameObject.GetComponentInParent<StatManager>() == null || gameObject.GetComponentInParent<EnemyStateManager>() == null))
                gameObject.GetComponentInParent<StatManager>().ModifyStat((gameObject.GetComponentInParent<EnemyStateManager>().isFlyer) ? "FLIGHT_FORCE" : "Speed", -addedSlow);

            empoweredBlightParticles.SetActive(true);
        }
    }



    private void StopEmpoweringDebuff()
    {
        isEmpowered = false;
        damage = manager.GetStats().ComputeValue("Blight DPS") * interval;
        currentInterval = interval;

        int addedSlow = Mathf.FloorToInt(manager.GetStats().ComputeValue("Blight Empowered Slow")) - Mathf.FloorToInt(manager.GetStats().ComputeValue("Blight Slow"));
        if (!(gameObject.GetComponentInParent<StatManager>() == null || gameObject.GetComponentInParent<EnemyStateManager>() == null))
            gameObject.GetComponentInParent<StatManager>().ModifyStat((gameObject.GetComponentInParent<EnemyStateManager>().isFlyer) ? "FLIGHT_FORCE" : "Speed", addedSlow);

        empoweredBlightParticles.SetActive(false);
    }
}
