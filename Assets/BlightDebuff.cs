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
    private float timer = 999f;
    public float duration = 999f;
    private SilasManager manager;
    private bool isEmpowered = false;
    private float empoweredDuration = 999f;



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
        timer = interval;
        damageContext.damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEmpowered)
        {
            empoweredDuration -= Time.deltaTime;
            if (empoweredDuration <= 0f)
            {
                StopEmpoweringDebuff();
            }
        }

        if (duration <= 0f)
        {
            if (isEmpowered) StopEmpoweringDebuff();
            health = gameObject.GetComponentInParent<Health>();
            if (health != null)
            {
                health.Damage(damageContext, PlayerID.instance.gameObject);
            }
            gameObject.GetComponentInParent<StatManager>().ModifyStat("Speed", Mathf.FloorToInt(manager.GetStats().ComputeValue("Blight Slow")));
            Destroy(gameObject);
        }
        duration -= Time.deltaTime;

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
        timer = interval;
    }



    public void ApplyDebuff(SilasManager manager)
    {
        float interval = 0.2f;
        this.manager = manager;
        damage = manager.GetStats().ComputeValue("Blight DPS") * interval;
        damageContext.damage = damage;
        this.interval = interval;
        duration = manager.GetStats().ComputeValue("Blight Duration");
        health = gameObject.GetComponentInParent<Health>();
        timer = 0f;

        gameObject.GetComponentInParent<StatManager>().ModifyStat("Speed", -Mathf.FloorToInt(manager.GetStats().ComputeValue("Blight Slow")));
    }

    public void AddDebuffTime(float time)
    {
        duration += time;
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
            damage = manager.GetStats().ComputeValue("Blight Empowered DPS") * interval;
            damageContext.damage = damage;
            empoweredDuration = manager.GetStats().ComputeValue("Blight Empowered Duration");

            int addedSlow = Mathf.FloorToInt(manager.GetStats().ComputeValue("Blight Empowered Slow")) - Mathf.FloorToInt(manager.GetStats().ComputeValue("Blight Slow"));
            gameObject.GetComponentInParent<StatManager>().ModifyStat("Speed", -addedSlow);

            empoweredBlightParticles.SetActive(true);
        }
    }


    private void StopEmpoweringDebuff()
    {
        isEmpowered = false;
        damage = manager.GetStats().ComputeValue("Blight DPS") * interval;
        damageContext.damage = damage;

        int addedSlow = Mathf.FloorToInt(manager.GetStats().ComputeValue("Blight Empowered Slow")) - Mathf.FloorToInt(manager.GetStats().ComputeValue("Blight Slow"));
        gameObject.GetComponentInParent<StatManager>().ModifyStat("Speed", addedSlow);

        empoweredBlightParticles.SetActive(false);
    }
}
