using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageIceShardAttack : EnemyProjectile
{
    [SerializeField] float damage;
    [SerializeField] float chargeTimeSec; // time suspended in air before launch
    GameObject initialTarget;
    GameObject attacker;
    Vector3 targetPos;
    bool launched;

    public void Initialize(GameObject targetObj, GameObject attacker)
    {
        initialTarget = targetObj;
        this.attacker = attacker;
        base.Init(attacker, initialTarget.transform.position);
        projectileDamage.damage = damage;
        projectileDamage.attacker = attacker;
    }
    void Update()
    {
        if (!launched && chargeTimeSec > 0)
            chargeTimeSec -= Time.deltaTime;
        else
        {
            Launch();
        }
    }
    new void FixedUpdate()
    {
        if (!launched)
        {
            Track();
        }
        else
        {
            base.FixedUpdate();
        }
    }
    void Launch()
    {
        launched = true;
        transform.parent = null;
    }
    void Track()
    {
        if (initialTarget != null)
        {
            targetPos = initialTarget.transform.position;
            dir = (targetPos - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (launched)
            base.ProcessCollision(collision.gameObject);
    }
}
