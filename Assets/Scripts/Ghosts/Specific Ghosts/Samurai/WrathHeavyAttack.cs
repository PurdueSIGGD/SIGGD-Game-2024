using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

/// <summary>
/// Thsi is the script that manages the "Wrath" value and activates the Samurai Heavy Attack Dash
/// </summary>
public class WrathHeavyAttack : MonoBehaviour
{
    public SamuraiManager manager;
    private Camera mainCamera;

    private PlayerStateMachine psm;
    public bool isCharging = false;
    private float chargingTime = 0f;
    public bool isPrimed = false;
    private float primedTime = 0f;

    private float primedHeavyDamage;

    private Rigidbody2D rb;
    private Vector2 desiredDashVelocity;
    private float desiredDashDist;
    private Vector2 desiredDashDest;
    private bool isDashing;
    private Vector2 startingDashLocation;
    private List<GameObject> enemiesDamaged;

    private Vector2 animationTransitionDest;
    private bool animationTransitionReached = false;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        psm = GetComponent<PlayerStateMachine>();
        rb = PlayerID.instance.GetComponent<Rigidbody2D>();
        enemiesDamaged = new List<GameObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDashing)
        {
            Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position + new Vector3(1f * Mathf.Sign(gameObject.transform.rotation.y), 0f, 0f), new Vector2(4, 2), 0, LayerMask.GetMask("Enemy"));
            foreach (Collider2D h in hit)
            {
                if (enemiesDamaged.Contains(h.gameObject)) continue;
                Health health = h.GetComponent<Health>();
                if (health)
                {
                    DamageContext context = manager.heavyDamageContext;
                    context.damage = Mathf.Lerp(manager.GetStats().ComputeValue("Heavy Attack Minimum Damage"),
                                                manager.GetStats().ComputeValue("Heavy Attack Maximum Damage"),
                                                manager.wrathPercent);
                    context.damage += primedHeavyDamage;
                    health.Damage(context, gameObject);
                    enemiesDamaged.Add(h.gameObject);
                }
            }

            // Animation Transition
            if (!animationTransitionReached && Mathf.Sign(transform.position.x - animationTransitionDest.x) == Mathf.Sign(desiredDashVelocity.x))
            {
                animationTransitionReached = true;
                psm.EnableTrigger("finishWrath");
                //Vector3 vfxPosition = gameObject.transform.position + new Vector3(1f * Mathf.Sign(gameObject.transform.rotation.y), 0f, 0f);
                //VFXManager.Instance.PlayVFX(VFX.PLAYER_LIGHT_ATTACK_1, vfxPosition, gameObject.transform.rotation, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
            }

            // Dash destination reached
            else if (Mathf.Sign(transform.position.x - desiredDashDest.x) == Mathf.Sign(desiredDashVelocity.x))
            {
                PlayerID.instance.GetComponent<Move>().PlayerGo();
                if (!GetComponent<Animator>().GetBool("p_grounded"))
                {
                    GetComponent<Move>().ApplyKnockback(rb.velocity.normalized, rb.velocity.magnitude, true);
                }

                //psm.EnableTrigger("finishWrath");
                StopSamuraiHeavyAttack();
                isDashing = false;
                enemiesDamaged.Clear();
                animationTransitionReached = false;

                gameObject.GetComponent<StatManager>().ModifyStat("Max Running Speed", -1 * 30);
                gameObject.GetComponent<StatManager>().ModifyStat("Running Accel.", -1 * 30);
                gameObject.GetComponent<Move>().UpdateRun();

                Vector3 vfxPosition = gameObject.transform.position + new Vector3(1f * Mathf.Sign(gameObject.transform.rotation.y), 0f, 0f);
                VFXManager.Instance.PlayVFX(VFX.PLAYER_LIGHT_ATTACK_1, vfxPosition, gameObject.transform.rotation, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
            }

            else
            {
                rb.velocity = desiredDashVelocity;
            }
        }

        if (isCharging && chargingTime > 0f) chargingTime -= Time.deltaTime;
        if (isCharging && chargingTime <= 0f) psm.EnableTrigger("OPT");

        if (isPrimed && primedTime > 0f) primedTime -= Time.deltaTime;
        if (isPrimed && primedTime <= 0f) psm.EnableTrigger("OPT");

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDiff = transform.position - mousePos;
        if (isCharging || isPrimed)
        {
            if (mouseDiff.x < 0) // update player facing direction
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (mouseDiff.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    //this functionn get called (via message from animator) when you enter the Heavy Charge up state
    public void StartHeavyChargeUp()
    {
        psm.SetLightAttack2Ready(true);
        chargingTime = manager.GetStats().ComputeValue("Heavy Charge Up Time");
        isCharging = true;
        AudioManager.Instance.SFXBranch.PlaySFXTrack("HeavyAttackWindUp");
        manager.decaying = false;
        manager.resetDecay = true;
        primedHeavyDamage = 0;
        manager.heavyDamageContext.damageStrength = DamageStrength.LIGHT;
        manager.heavyDamageContext.extraContext = "";
    }

    public void StopHeavyChargeUp()
    {
        isCharging = false;
        chargingTime = 0f;
    }

    public void StartHeavyPrimed()
    {
        primedTime = manager.GetStats().ComputeValue("Heavy Primed Autofire Time");
        isPrimed = true;
        AudioManager.Instance.SFXBranch.PlaySFXTrack("HeavyAttackPrimed");

        primedHeavyDamage = Mathf.Lerp(manager.GetStats().ComputeValue("Primed Attack Extra Minimum Damage"), 
                                       manager.GetStats().ComputeValue("Primed Attack Extra Maximum Damage"), 
                                       manager.wrathPercent);
        manager.heavyDamageContext.damageStrength = DamageStrength.HEAVY;
        manager.heavyDamageContext.extraContext = "Full Charge";
    }

    public void StopHeavyPrimed()
    {
        isPrimed = false;
        primedTime = 0f;
    }

    //this function get called (via message form animator) when you enter the Heavy Attack state
    public void StartHeavyAttack()
    {
        psm.ConsumeHeavyAttackInput();
        AudioManager.Instance.VABranch.PlayVATrack(PartyManager.instance.selectedGhost + " Heavy Attack");
    }

    public void ExecuteHeavyAttack()
    {
        psm.SetLightAttack2Ready(true);
        AudioManager.Instance.SFXBranch.GetSFXTrack("Akihito-Dash Attack").SetPitch(manager.wrathPercent, 1f);
        AudioManager.Instance.SFXBranch.PlaySFXTrack("Akihito-Dash Attack");

        Vector2 dir = Vector2.zero;
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < PlayerID.instance.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            dir = new Vector2(-1, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            dir = new Vector2(1, 0);
        }

        startingDashLocation = transform.position;
        desiredDashDist = Mathf.Lerp(manager.GetStats().ComputeValue("Heavy Attack Minimum Travel Distance"), 
                                     manager.GetStats().ComputeValue("Heavy Attack Maximum Travel Distance"), 
                                     manager.wrathPercent);
        desiredDashDest = new (transform.position.x + desiredDashDist * dir.x, transform.position.y);
        //targetedEnemyDest = new(transform.position.x + (desiredDashDist - 1f) * dir.x, transform.position.y);

        // Enemy hit distance modifier
        RaycastHit2D enemyHit = Physics2D.Raycast(transform.position, dir, desiredDashDist + 2f, LayerMask.GetMask("Enemy"));
        if (enemyHit)
        {
            float displacement = transform.position.x - enemyHit.point.x;
            float onHitDist = manager.GetStats().ComputeValue("Heavy Attack On-hit Final Travel Distance");
            desiredDashDist = Mathf.Abs(displacement) + onHitDist;
            desiredDashDest = enemyHit.point + new Vector2(onHitDist * Mathf.Sign(displacement) * -1, 0);
            //targetedEnemyDest = enemyHit.point + new Vector2(1f * Mathf.Sign(displacement), 0);
        }

        // Guaranteed distance floor (make sure dash distance can't be reduced too much by enemy hit modifier)
        if (desiredDashDist < manager.GetStats().ComputeValue("Heavy Attack Guaranteed Travel Distance"))
        {
            desiredDashDist = manager.GetStats().ComputeValue("Heavy Attack Guaranteed Travel Distance");
            desiredDashDest = desiredDashDest = new(transform.position.x + desiredDashDist * dir.x, transform.position.y);
        }

        // Level geometry distance modifier
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, dir, desiredDashDist, LayerMask.GetMask("Ground"));
        if (groundHit)
        {
            desiredDashDist = Mathf.Abs(transform.position.x - groundHit.point.x);
            desiredDashDest = groundHit.point + new Vector2(1.5f * Mathf.Sign(transform.position.x - groundHit.point.x), 0);
        }

        animationTransitionDest = new (transform.position.x + (desiredDashDist - 0.5f) * dir.x, transform.position.y);

        desiredDashVelocity = manager.GetStats().ComputeValue("Heavy Attack Travel Speed") * dir;
        rb.isKinematic = true;
        PlayerID.instance.GetComponent<Move>().PlayerStop();
        isDashing = true;

        GameplayEventHolder.OnAbilityUsed?.Invoke(manager.onDashContext);
    }

    //this function get called (via message form animator) when you exit the Heavy Attack state
    public void StopHeavyAttack() 
    {
        //rb.velocity *= manager.GetStats().ComputeValue("Heavy Attack Post Dash Momentum Fraction");


        
        /*
        PlayerID.instance.GetComponent<Move>().PlayerGo();
        if (!GetComponent<Animator>().GetBool("p_grounded"))
        {
            GetComponent<Move>().ApplyKnockback(rb.velocity.normalized, rb.velocity.magnitude, true);
        }
        */
        


        /*
        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position, new Vector2(3, 2), 0, LayerMask.GetMask("Enemy"));
        foreach (Collider2D h in hit)
        {
            Health health = h.GetComponent<Health>();
            if (health)
            {
                DamageContext context = manager.heavyDamageContext;
                context.damage = Mathf.Lerp(manager.GetStats().ComputeValue("Heavy Attack Minimum Damage"),
                                            manager.GetStats().ComputeValue("Heavy Attack Maximum Damage"),
                                            manager.wrathPercent);
                context.damage += primedHeavyDamage;
                health.Damage(context, gameObject);
            }
        }
        */


        /*
        ResetWrath();
        if (manager.resetDecay)
        {
            manager.decaying = true;
            manager.resetDecay = false;
        }
        */
    }

    public void StartSamuraiHeavySlash()
    {
        /*
        gameObject.GetComponent<StatManager>().ModifyStat("Max Running Speed", -1 * 50);
        gameObject.GetComponent<StatManager>().ModifyStat("Running Accel.", -1 * 50);
        gameObject.GetComponent<Move>().UpdateRun();

        Vector3 vfxPosition = gameObject.transform.position + new Vector3(1f * Mathf.Sign(gameObject.transform.rotation.y), 0f, 0f);
        VFXManager.Instance.PlayVFX(VFX.PLAYER_LIGHT_ATTACK_1, vfxPosition, gameObject.transform.rotation, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
        */
    }

    public void StopSamuraiHeavySlash()
    {
        gameObject.GetComponent<StatManager>().ModifyStat("Max Running Speed", 30);
        gameObject.GetComponent<StatManager>().ModifyStat("Running Accel.", 30);
        gameObject.GetComponent<Move>().UpdateRun();

        //StopSamuraiHeavyAttack();
        //isDashing = false;
        //enemiesDamaged.Clear();

        //rb.velocity *= manager.GetStats().ComputeValue("Heavy Attack Post Dash Momentum Fraction");

        /*
        PlayerID.instance.GetComponent<Move>().PlayerGo();
        if (!GetComponent<Animator>().GetBool("p_grounded"))
        {
            GetComponent<Move>().ApplyKnockback(rb.velocity.normalized, rb.velocity.magnitude, true);
        }
        */

        /*
        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position, new Vector2(3, 2), 0, LayerMask.GetMask("Enemy"));
        foreach (Collider2D h in hit)
        {
            Health health = h.GetComponent<Health>();
            if (health)
            {
                DamageContext context = manager.heavyDamageContext;
                context.damage = Mathf.Lerp(manager.GetStats().ComputeValue("Heavy Attack Minimum Damage"),
                                            manager.GetStats().ComputeValue("Heavy Attack Maximum Damage"),
                                            manager.wrathPercent);
                context.damage += primedHeavyDamage;
                health.Damage(context, gameObject);
            }
        }
        */

        ResetWrath();
        if (manager.resetDecay)
        {
            manager.decaying = true;
            manager.resetDecay = false;
        }
    }

    private void StopSamuraiHeavyAttack()
    {
        rb.isKinematic = false;
        rb.velocity *= manager.GetStats().ComputeValue("Heavy Attack Post Dash Momentum Fraction");
    }

    //Used to empty the remaining wrath percentage
    public void ResetWrath()
    {
        manager.wrathPercent = 0.0f;
    }

    public float GetWrathPercent()
    {
        return manager.wrathPercent;
    }

    public void SetWrathPercent(float newWrathPercent)
    {
        manager.wrathPercent = newWrathPercent;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawCube(transform.position, new Vector3(2.5f, 1, 0));
    //}
}
