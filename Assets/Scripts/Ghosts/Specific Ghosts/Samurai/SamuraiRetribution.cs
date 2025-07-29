using System.Collections;
using UnityEngine;

public class SamuraiRetribution : MonoBehaviour
{
    [HideInInspector] public SamuraiManager manager;
    private PlayerStateMachine psm;
    private bool parrying;
    private bool parrySuccess;
    private bool parryChaining = false;
    private Camera mainCamera;

    private float parrySFXPitch = 0f;
    private float parrySFXPitchTime = 0f;
    private int currentParrySFX = 0;

    void Start()
    {
        psm = GetComponent<PlayerStateMachine>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageFilter.Add(ParryingFilter);
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageFilter.Remove(ParryingFilter);
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
        psm.ConsumeSpecialInput();

        parrySuccess = false;
        parrying = true;
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
        if (!parryChaining)
        {
            //AudioManager.Instance.SFXBranch.PlaySFXTrack("Akihito-Parry Success");
            AudioManager.Instance.SFXBranch.PlaySFXTrack("Akihito-Parry Failure");
        }
    }

    private void ParryingProjectiles()
    {
        Collider2D[] coll = Physics2D.OverlapBoxAll(transform.position, new Vector2(2, 3), 0, LayerMask.GetMask("Projectiles", "Enemy"));
        foreach (Collider2D coll2d in coll)
        {
            if (!coll2d.gameObject.CompareTag("Enemy"))
            {
                EnemyProjectile projectile = coll2d.gameObject.GetComponent<EnemyProjectile>();
                if (projectile && !projectile.parried)
                {
                    projectile.projectileDamage.actionID = ActionID.SAMURAI_SPECIAL;
                    //projectile.projectileDamage.damage *= manager.GetStats().ComputeValue("Melee Parry Bonus Damage per Incoming Attack Damage");
                    projectile.GetStats().ModifyStat("Damage", Mathf.CeilToInt((manager.GetStats().ComputeValue("Melee Parry Bonus Damage per Incoming Attack Damage") * 100f) - 100f));
                    projectile.projectileDamage.damageStrength = DamageStrength.MINOR;
                    projectile.target = "Enemy";
                    projectile.SwitchDirections();
                    projectile.SetParried(true);
                    NotifyParrySuccess(coll2d.transform.position);
                }
            }
        }
    }

    public void StopParry()
    {
        manager.startSpecialCooldown();
        if (parrySuccess || parryChaining)
        {
            manager.setSpecialCooldown(manager.GetStats().ComputeValue("Parry Success Special Cooldown"));
            manager.GetComponent<SamuraiUIDriver>().specialAbilityUIManager.pingAbility();
        }
        parryChaining = parrySuccess;

        parrying = false;
        parrySuccess = false;
        gameObject.GetComponent<StatManager>().ModifyStat("Max Running Speed", Mathf.CeilToInt(manager.GetStats().ComputeValue("Parry Slow Percent")));
        gameObject.GetComponent<StatManager>().ModifyStat("Running Accel.", Mathf.CeilToInt(manager.GetStats().ComputeValue("Parry Slow Percent")));
        gameObject.GetComponent<Move>().UpdateRun();
    }

    public void ParryingFilter(ref DamageContext context)
    {
        if (parrying && context.attacker.CompareTag("Enemy") && context.victim.CompareTag("Player"))
        {
            DamageContext newContext = manager.specialMeleeParryContext;
            newContext.attacker = gameObject;
            newContext.victim = context.attacker;
            newContext.damage = (context.damage * manager.GetStats().ComputeValue("Melee Parry Bonus Damage per Incoming Attack Damage"))
                                + manager.GetStats().ComputeValue("Melee Parry Base Damage");

            EnemyStateManager esm = context.attacker.GetComponent<EnemyStateManager>();
            esm.Stun(newContext, manager.GetStats().ComputeValue("Melee Parry Stun Time"));
            esm.ApplyKnockback(Vector3.up, 6f, 0.3f);
            esm.ApplyKnockback(context.attacker.transform.position - context.victim.transform.position, 3.8f, 0.3f);
            context.attacker.GetComponent<Health>().Damage(newContext, gameObject);

            context.damage = 0;
            NotifyParrySuccess(context.victim.transform.position + (Vector3.Normalize(context.attacker.transform.position - context.victim.transform.position) * 1.5f));
        }
    }

    private void NotifyParrySuccess(Vector3 position)
    {
        ActionContext newContext = manager.onParryContext;
        newContext.extraContext = "Parry Success";
        parrySuccess = true;
        parryChaining = true;
        GameplayEventHolder.OnAbilityUsed?.Invoke(newContext);
        psm.EnableTrigger("OPT");

        // VFX
        GameObject surfaceExplosion = Instantiate(manager.parrySuccessVFX, position, Quaternion.identity);
        surfaceExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(1.5f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor);

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
