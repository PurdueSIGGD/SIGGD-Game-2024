using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SamuraiRetribution : MonoBehaviour
{
    private bool parrying;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (parrying)
        {
            ParryingProjectiles();
        }
    }

    public void StartDash()
    {
        Debug.Log("StartParrying");

        parrying = true;
        GameplayEventHolder.OnDamageFilter.Add(ParryingFilter);
    }

    private void ParryingProjectiles()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = Vector3.zero;
        if (mousePos.x < transform.position.x)
        {
            dir = new Vector3(-1, 0, 0);
        }
        else
        {
            dir = new Vector3(1, 0, 0);
        }

        Collider2D[] coll = Physics2D.OverlapBoxAll(dir + transform.position, new Vector2(2, 3), 0, LayerMask.GetMask("Projectiles", "Enemy"));

        Debug.DrawLine(transform.position + dir * 2 + new Vector3(0, -1.5f, 0), transform.position + dir * 2 + new Vector3(0, 1.5f, 0), Color.blue, Time.deltaTime);
        Debug.DrawLine(transform.position + new Vector3(0, -1.5f, 0), transform.position + new Vector3(0, 1.5f, 0), Color.blue, Time.deltaTime);

        foreach (Collider2D coll2d in coll)
        {
            if (!coll2d.gameObject.CompareTag("Enemy"))
            {
                coll2d.gameObject.GetComponent<EnemyProjectile>().target = "Enemy";
                coll2d.gameObject.GetComponent<EnemyProjectile>().SwitchDirections();
            }
        }
    }

    public void StopDash()
    {
        Debug.Log("Stop Parrying");
        GameplayEventHolder.OnDamageFilter.Remove(ParryingFilter);
    }

    public void ParryingFilter(ref DamageContext context)
    {
        if (context.attacker.CompareTag("Enemy"))
        {
            DamageContext newContext = context;
            newContext.attacker = gameObject;
            newContext.victim = context.attacker;
            context.attacker.GetComponent<Health>().Damage(newContext, gameObject);
            context.damage = 0;
        }
    }
}
