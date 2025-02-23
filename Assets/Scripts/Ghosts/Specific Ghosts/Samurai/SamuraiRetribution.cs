using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SamuraiRetribution : MonoBehaviour
{
    private bool parrying;
    private Camera mainCamera;

    public void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void StartDash()
    {
        GameplayEventHolder.OnDamageFilter.Add(ParryingFilter);

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

        Debug.DrawLine(transform.position + dir*2 + new Vector3(0, -1.5f, 0), transform.position + dir*2 + new Vector3(0, 1.5f, 0), Color.blue, 1.0f);
        Debug.DrawLine(transform.position+ new Vector3(0, -1.5f, 0), transform.position+ new Vector3(0, 1.5f, 0), Color.blue, 1.0f);

        foreach (Collider2D coll2d in coll)
        {
            if (!coll2d.gameObject.CompareTag("Enemy"))
            {
                coll2d.gameObject.GetComponent<EnemyProjectile>().target = "Enemy";
                coll2d.gameObject.GetComponent<EnemyProjectile>().SwitchDirections();
            }
        }

        Debug.Log("Start Parrying");
    }

    public void StopDash()
    {
        GameplayEventHolder.OnDamageFilter.Remove(ParryingFilter);

        Debug.Log("Stop Parrying");
    }

    public void ParryingFilter(DamageContext context)
    {
        Debug.Log(context.attacker.tag);
        if (context.attacker.CompareTag("Enemy"))
        {
            DamageContext newContext = context;
            newContext.attacker = gameObject;
            newContext.victim = context.attacker;
            context.attacker.GetComponent<Health>().Damage(newContext, gameObject);
            context.damage = 0;
            Debug.Log("Parried");
        }
    }
}
