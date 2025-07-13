using System.Collections;
using UnityEngine;

public class SamuraiRetribution : MonoBehaviour
{
    [HideInInspector] public SamuraiManager manager;
    private PlayerStateMachine psm;
    private bool parrying;
    private bool parrySuccess;
    private Camera mainCamera;

    private float parrySFXPitch = 0f;
    private float parrySFXPitchTime = 0f;
    private int currentParrySFX = 0;

    void Start()
    {
        psm = GetComponent<PlayerStateMachine>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (parrySFXPitch > 0f && parrySFXPitchTime > 0f) parrySFXPitchTime -= Time.deltaTime;
        if (parrySFXPitch > 0f && parrySFXPitchTime <= 0f) parrySFXPitch = 0f;

        if (parrying)
        {
            ParryingProjectiles();
        }

        if (manager != null)
        {
            if (manager.getSpecialCooldown() > 0)
            {
                psm.OnCooldown("c_special");
            }
            else
            {
                psm.OffCooldown("c_special");
            }
        }
    }

    public void StartParry()
    {
        //parrying = true;
        StartCoroutine(ParryDuration());
        GameplayEventHolder.OnDamageFilter.Add(ParryingFilter);
        gameObject.GetComponent<StatManager>().ModifyStat("Max Running Speed", -1 * Mathf.CeilToInt(manager.GetStats().ComputeValue("Parry Slow Percent")));
        gameObject.GetComponent<StatManager>().ModifyStat("Running Accel.", -1 * Mathf.CeilToInt(manager.GetStats().ComputeValue("Parry Slow Percent")));
        gameObject.GetComponent<Move>().UpdateRun();

        // VFX
        Vector3 vfx1Position = gameObject.transform.position + new Vector3(1f * Mathf.Sign(gameObject.transform.rotation.y), 0f, 0f);
        Vector3 vfx2Position = gameObject.transform.position + new Vector3(0f * Mathf.Sign(gameObject.transform.rotation.y), 0f, 0f);
        Quaternion vfx2Rotation = gameObject.transform.rotation * new Quaternion(0f, 0f, 180f, 0f);
        VFXManager.Instance.PlayVFX(VFX.PLAYER_LIGHT_ATTACK_2, vfx1Position, gameObject.transform.rotation, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
        VFXManager.Instance.PlayVFX(VFX.PLAYER_LIGHT_ATTACK_2, vfx2Position, vfx2Rotation, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);

        // SFX
        AudioManager.Instance.SFXBranch.PlaySFXTrack("Akihito-Parry Success");
        AudioManager.Instance.SFXBranch.PlaySFXTrack("Akihito-Parry Failure");
        //parrySFXPitch = 0f;
    }

    private IEnumerator ParryDuration()
    {
        parrying = true;
        yield return new WaitForSeconds(manager.GetStats().ComputeValue("Parry Duration"));
        parrying = false;
        gameObject.GetComponent<StatManager>().ModifyStat("Max Running Speed", Mathf.CeilToInt(manager.GetStats().ComputeValue("Parry Slow Percent")));
        gameObject.GetComponent<StatManager>().ModifyStat("Running Accel.", Mathf.CeilToInt(manager.GetStats().ComputeValue("Parry Slow Percent")));
        gameObject.GetComponent<Move>().UpdateRun();
    }

    private void ParryingProjectiles()
    {
        /*
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
        */

        Collider2D[] coll = Physics2D.OverlapBoxAll(transform.position, new Vector2(2, 3), 0, LayerMask.GetMask("Projectiles", "Enemy"));

        //Debug.DrawLine(transform.position + dir * 2 + new Vector3(0, -1.5f, 0), transform.position + dir * 2 + new Vector3(0, 1.5f, 0), Color.blue, Time.deltaTime);
        //Debug.DrawLine(transform.position + new Vector3(0, -1.5f, 0), transform.position + new Vector3(0, 1.5f, 0), Color.blue, Time.deltaTime);

        foreach (Collider2D coll2d in coll)
        {
            if (!coll2d.gameObject.CompareTag("Enemy"))
            {
                EnemyProjectile projectile = coll2d.gameObject.GetComponent<EnemyProjectile>();
                if (projectile && !projectile.parried)
                {
                    projectile.target = "Enemy";
                    projectile.SwitchDirections();
                    projectile.SetParried(true);
                    projectile.projectileDamage.actionID = ActionID.SAMURAI_SPECIAL;
                    projectile.projectileDamage.damage = projectile.projectileDamage.damage * manager.GetStats().ComputeValue("Melee Parry Bonus Damage per Incoming Attack Damage");
                    projectile.projectileDamage.damageStrength = DamageStrength.MINOR;
                    NotifyParrySuccess();
                }
            }
        }

        /*
        if (parrySuccess)
        {
            psm.EnableTrigger("finishParry");
        }
        */
    }

    public void StopParry()
    {
        //gameObject.GetComponent<StatManager>().ModifyStat("Max Running Speed", Mathf.CeilToInt(manager.GetStats().ComputeValue("Parry Slow Percent")));
        //gameObject.GetComponent<StatManager>().ModifyStat("Running Accel.", Mathf.CeilToInt(manager.GetStats().ComputeValue("Parry Slow Percent")));
        //gameObject.GetComponent<Move>().UpdateRun();
        manager.startSpecialCooldown();
        if (parrySuccess)
        {
            manager.setSpecialCooldown(manager.GetStats().ComputeValue("Parry Success Special Cooldown"));
        }
        parrying = false;
        parrySuccess = false;
        GameplayEventHolder.OnDamageFilter.Remove(ParryingFilter);
    }

    public void ParryingFilter(ref DamageContext context)
    {
        if (context.attacker.CompareTag("Enemy") && context.victim.CompareTag("Player"))
        {
            //DamageContext newContext = context;
            DamageContext newContext = manager.specialMeleeParryContext;
            newContext.attacker = gameObject;
            newContext.victim = context.attacker;
            //newContext.actionID = ActionID.SAMURAI_SPECIAL;
            newContext.damage = (context.damage * manager.GetStats().ComputeValue("Melee Parry Bonus Damage per Incoming Attack Damage"))
                                + manager.GetStats().ComputeValue("Melee Parry Base Damage");

            EnemyStateManager esm = context.attacker.GetComponent<EnemyStateManager>();
            esm.Stun(newContext, manager.GetStats().ComputeValue("Melee Parry Stun Time"));
            esm.ApplyKnockback(Vector3.up, 4f, 0.3f);
            esm.ApplyKnockback(context.attacker.transform.position - context.victim.transform.position, 3.8f, 0.3f);
            context.attacker.GetComponent<Health>().Damage(newContext, gameObject);

            context.damage = 0;
            NotifyParrySuccess();

            // once a parry is successful, exit parry anim
            //psm.EnableTrigger("finishParry");
        }
    }

    private void NotifyParrySuccess()
    {
        ActionContext newContext = manager.onParryContext;
        newContext.extraContext = "Parry Success";
        parrySuccess = true;
        GameplayEventHolder.OnAbilityUsed?.Invoke(newContext);

        // SFX
        AudioManager.Instance.SFXBranch.GetSFXTrack("Akihito-Relentless Fury").SetPitch(parrySFXPitch, 1f);
        AudioManager.Instance.SFXBranch.GetSFXTrack("Akihito-Relentless Fury 2").SetPitch(parrySFXPitch, 1f);
        AudioManager.Instance.SFXBranch.GetSFXTrack("Akihito-Relentless Fury 3").SetPitch(parrySFXPitch, 1f);
        switch (currentParrySFX)
        {
            case 0:
                AudioManager.Instance.SFXBranch.PlaySFXTrack("Akihito-Relentless Fury");
                currentParrySFX = 1;
                break;
            case 1:
                AudioManager.Instance.SFXBranch.PlaySFXTrack("Akihito-Relentless Fury 2");
                currentParrySFX = 2;
                break;
            case 2:
                AudioManager.Instance.SFXBranch.PlaySFXTrack("Akihito-Relentless Fury 3");
                currentParrySFX = 0;
                break;
            default:
                AudioManager.Instance.SFXBranch.PlaySFXTrack("Akihito-Relentless Fury");
                currentParrySFX = 1;
                break;
        }
        parrySFXPitch = Mathf.Min(parrySFXPitch + 0.5f, 1f);
        parrySFXPitchTime = 1.5f;
    }
}
