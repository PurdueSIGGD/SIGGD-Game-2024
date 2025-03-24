using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

/// <summary>
/// Defines functions for PoliceChief Basic attack to be called through state machine behavior
/// Note: The effective lower bound for cooldown between shots is the animation length, as the state machine will
/// not proceed until the animation is finished running.
/// </summary>
public class PoliceChiefBasic : MonoBehaviour
{
    PlayerStateMachine psm;
    DamageContext damage;
    float range;
    float cooldownTime;
    float width;
    Camera mainCamera;
    LineRenderer tracer; // visual purposes only
    void Start()
    {
        ResetSidearm();
        psm = PlayerID.instance.GetComponent<PlayerStateMachine>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    public void SetVars(StatManager stats, LineRenderer tracer)
    {
        cooldownTime = stats.ComputeValue("BASIC_COOLDOWN");
        damage.damage = stats.ComputeValue("BASIC_DAMAGE");
        range = stats.ComputeValue("BASIC_RANGE");
        width = stats.ComputeValue("BASIC_WIDTH");
        this.tracer = tracer;
    }
    public void FireSidearm()
    {
        GetComponent<Move>().PlayerStop();

        Vector2 rawDir = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Vector2 dir = rawDir.normalized;
        Vector2 perp = Vector2.Perpendicular(dir) * width / 2;

        transform.rotation = dir.x > 0 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);

        List<RaycastHit2D> hits = new List<RaycastHit2D>
        {
            Physics2D.Raycast((Vector2)transform.position - perp, dir, range, LayerMask.GetMask("Enemy", "Ground")),
            Physics2D.Raycast(transform.position, dir, range, LayerMask.GetMask("Enemy", "Ground")),
            Physics2D.Raycast((Vector2)transform.position + perp, dir, range, LayerMask.GetMask("Enemy", "Ground"))
        };

        Debug.DrawRay((Vector2)transform.position - perp, dir * range);
        Debug.DrawRay(transform.position, dir * range);
        Debug.DrawRay((Vector2)transform.position + perp, dir * range);

        bool hitSomething = false;
        for (int i = 0; i < hits.Count; i++)
        {
            if (hits[i])
            {
                DrawLine(tracer, transform.position, hits[i].point);
                if (hits[i].collider.gameObject.CompareTag("Enemy"))
                {
                    hits[i].collider.gameObject.GetComponent<Health>().Damage(damage, gameObject);
                }
                hitSomething = true;
                break;
            }
        }
        if (!hitSomething)
        {
            DrawLine(tracer, transform.position, dir * range);
        }

        StartCoroutine(BulletTime(0.2f));
        StartCoroutine(SidearmCooldown(cooldownTime));
    }
    IEnumerator BulletTime(float time)
    {
        yield return new WaitForSeconds(time);
        tracer.positionCount = 0;
    }
    IEnumerator SidearmCooldown(float time)
    {
        print("Sidearm cooldown");
        psm.OnCooldown("c_sidearm");
        yield return new WaitForSeconds(time);
        psm.OffCooldown("c_sidearm");
        print("Done!");
    }
    public void StopSidearm()
    {
        GetComponent<Move>().PlayerGo();
    }
    public void ResetSidearm()
    {
        StopAllCoroutines();
    }
    public void DrawLine(LineRenderer render, Vector2 start, Vector2 end)
    {
        render.positionCount = 2;
        render.SetPosition(0, new Vector3(start.x, start.y, 0));
        render.SetPosition(1, new Vector3(end.x, end.y, 0));
    }
}
