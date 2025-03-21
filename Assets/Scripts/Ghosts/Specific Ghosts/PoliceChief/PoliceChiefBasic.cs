using System.Collections;
using System.Collections.Generic;
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
    Camera mainCamera;
    void Start()
    {
        ResetSidearm();
        psm = PlayerID.instance.GetComponent<PlayerStateMachine>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    public void SetStats(StatManager stats)
    {
        cooldownTime = stats.ComputeValue("BASIC_COOLDOWN");
        damage.damage = stats.ComputeValue("BASIC_DAMAGE");
        range = stats.ComputeValue("BASIC_RANGE");
    }
    public void FireSidearm()
    {
        GetComponent<Move>().PlayerStop();

        Vector2 rawDir = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Vector2 dir = rawDir.normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, range, LayerMask.GetMask("Enemy", "Ground"));
        Debug.DrawRay(transform.position, dir * range);
        if (hit && hit.collider.gameObject.CompareTag("Enemy"))
        {
            hit.collider.gameObject.GetComponent<Health>().Damage(damage, gameObject);
        }

        StartCoroutine(SidearmCooldown(cooldownTime));
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
}
