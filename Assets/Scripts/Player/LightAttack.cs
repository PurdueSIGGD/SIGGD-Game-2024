using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/// <summary>
/// This is the script that contains the code form the player's melee attack
/// </summary>
public class LightAttack : MonoBehaviour, IStatList
{
    [SerializeField] float range = 1.2f; // radius of the attack cone
    [SerializeField] float angle = 80f; // angle of the attack cone
    [SerializeField] DamageContext lightDamage;
    [SerializeField] StatManager.Stat[] statList;
    [SerializeField] float cooldown = 1; // Cooldown of player attack
    [SerializeField] float rayCount = 6; // number of rays used to check for collision
    [SerializeField] LayerMask attackMask;

    private HashSet<int> hits; // stores which targets have already been hit in one attack
    private Camera mainCamera;
    private StatManager stats;
    private PlayerStateMachine playerStateMachine;
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 lastMousePos;

    private bool isSkyLightAttackDashing = false;
    private Vector2 skyLightAttackVelocity;
    private bool isUpSkyAttack = false;

    // Attack Info
    private Vector2 orig;
    private float deltaAngle;
    private Vector2 center;

    private void Start()
    {
        hits = new HashSet<int>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        stats = gameObject.GetComponent<StatManager>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        lightDamage.damage = stats.ComputeValue("Damage");
        skyLightAttackVelocity = Vector2.zero;
    }

    private void Update()
    {
        if (isSkyLightAttackDashing)
        {
            rb.velocity = skyLightAttackVelocity;
        }

        if (!animator.GetBool("air_light_ready") && animator.GetBool("p_grounded"))
        {
            animator.SetBool("air_light_ready", true);
        }
    }



    /// <summary>
    /// Damage enemies in a cone shape in the direction of the cursor
    /// </summary>
    public void StartLightAttack()
    {
        AudioManager.Instance.VABranch.PlayVATrack(PartyManager.instance.selectedGhost + " Light Attack");
        GetComponent<Move>().PlayerStop();
    }



    public void InitializeLightAttack()
    {
        
        playerStateMachine.SetLightAttackRecoveryState(false);
        //playerStateMachine.ConsumeLightAttackInput();
        playerStateMachine.SetLightAttack2Ready(!playerStateMachine.isLightAttack2Ready);

        float halfAngle = angle / 2; // angle above and below the centerline of the attack cone
        deltaAngle = halfAngle / rayCount * 2; // change in degree between each ray

        orig = transform.position;
        Vector2 mouseDiff = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position); // center ray
        float m_angle = Mathf.Atan2(mouseDiff.x, mouseDiff.y) + Mathf.PI;
        center = new Vector2(Mathf.Sign(mouseDiff.x), 0);

        if (center.x == 1) // update player facing direction
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (center.x == -1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (m_angle > 3 * Mathf.PI / 4 && m_angle < 5 * Mathf.PI / 4)
        {
            center = new Vector2(0, 1);
        }
        center = center.normalized;
    }



    public void EnterLightAttackRecovery()
    {
        playerStateMachine.SetLightAttackRecoveryState(true);
    }



    public void ExecuteLightAttack()
    {
#if DEBUG
        Debug.DrawLine(orig, center * range + orig, Color.red, 0.25f);
#endif
        GetComponent<PlayerParticles>().PlayLightAttackVFX(false);
        CameraShake.instance.Shake(0.02f, 10f, 0, 10, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
        for (int i = 1; i <= rayCount / 2; i++)
        {
            CastDamageRay(orig, deltaAngle * i, center);
            CastDamageRay(orig, deltaAngle * -i, center);
        }
        AudioManager.Instance.SFXBranch.PlaySFXTrack("LightAttack");
        hits.Clear(); // re-enable damage to all hit enemy
    }



    public void ExecuteUpLightAttack()
    {
#if DEBUG
        Debug.DrawLine(orig, center * range + orig, Color.red, 0.25f);
#endif
        GetComponent<PlayerParticles>().PlayLightAttackVFX(true);
        playerStateMachine.SetLightAttack2Ready(true);

        CameraShake.instance.Shake(0.02f, 10f, 0, 10, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
        for (int i = 1; i <= rayCount / 2; i++)
        {
            CastDamageRay(orig, deltaAngle * i, center);
            CastDamageRay(orig, deltaAngle * -i, center);
        }
        AudioManager.Instance.SFXBranch.PlaySFXTrack("LightAttack");
        hits.Clear(); // re-enable damage to all hit enemy
    }



    public void EndLightAttack()
    {
        if (playerStateMachine.lightAttackQueued) return;
        playerStateMachine.SetLightAttackRecoveryState(false);
        GetComponent<Move>().PlayerGo();
        GetComponent<Animator>().SetTrigger("OPT");
    }



    public void StopLightAttack()
    {

    }





    // SKY ATTACKS

    public void StartSkyLightAttack()
    {
        AudioManager.Instance.VABranch.PlayVATrack(PartyManager.instance.selectedGhost + " Light Attack");

        bool isSkyDash = animator.GetBool("air_light_ready");
        animator.SetBool("air_light_ready", false);
        isUpSkyAttack = false;
        Vector2 orig = transform.position;
        Vector2 mouseDiff = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position); // center ray
        float m_angle = Mathf.Atan2(mouseDiff.x, mouseDiff.y) + Mathf.PI;

        Vector2 center = new Vector2(Mathf.Sign(mouseDiff.x), 0);
        float upDashXVelocity = 0f;

        if (center.x == 1) // update player facing direction
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (center.x == -1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        // Up Sky Attack
        if (m_angle > 3 * Mathf.PI / 4 && m_angle < 5 * Mathf.PI / 4)
        {
            isUpSkyAttack = true;
            center = new Vector2(0, 1);
            upDashXVelocity = rb.velocity.x;
        }

        // Down Sky Attack
        if (m_angle < Mathf.PI / 4 || m_angle > 7 * Mathf.PI / 4)
        {
            center = new Vector2(0, -1);
        }

        center = center.normalized;
        lastMousePos = center;

        //Debug.Log(center * 10);
        //GetComponent<Rigidbody2D>().AddForce(center * 10, ForceMode2D.Impulse);
        //GetComponent<Move>().ApplyKnockback(center, 10, true);

        //GetComponent<Move>().PlayerStop();
        //Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 direction = ((Vector2)mousePos - (Vector2)transform.position).normalized;

        if (!isSkyDash) return;

        //Vector2 displacement = center * 3f;
        GetComponent<Move>().PlayerStop();
        skyLightAttackVelocity = (center * stats.ComputeValue("Air Attack Dash Speed")) + new Vector2(upDashXVelocity, 0f);
        //skyLightAttackVelocity = new Vector2(skyLightAttackVelocity.x + upDashXVelocity, skyLightAttackVelocity.y);
        isSkyLightAttackDashing = true;
    }



    public void EndSkyLightAttackDash()
    {
        if (!isSkyLightAttackDashing) return;
        isSkyLightAttackDashing = false;
        GetComponent<Move>().PlayerGo();
        rb.velocity *= (isUpSkyAttack) ? stats.ComputeValue("Post Up Air Attack Dash Momentum Fraction") : stats.ComputeValue("Post Side Air Attack Dash Momentum Fraction");
        GetComponent<Move>().ApplyKnockback(rb.velocity.normalized, rb.velocity.magnitude, true);
    }



    public void ExecuteSkyLightAttack()
    {
        GetComponent<PlayerParticles>().PlayLightAttackVFX(false);
        CameraShake.instance.Shake(0.02f, 10f, 0, 10, new Vector2(Random.Range(-0.5f, 0.5f), 1f));

        float halfAngle = angle / 2; // angle above and below the centerline of the attack cone
        float deltaAngle = halfAngle / rayCount * 2; // change in degree between each ray

        Vector2 orig = transform.position;
#if DEBUG        
        Debug.DrawLine(orig, lastMousePos * range + orig, Color.red, 1.0f);
#endif

        for (int i = 1; i <= rayCount / 2; i++)
        {
            CastDamageRay(orig, deltaAngle * i, lastMousePos);
            CastDamageRay(orig, deltaAngle * -i, lastMousePos);
        }
        AudioManager.Instance.SFXBranch.PlaySFXTrack("AirAttack");
        hits.Clear(); // re-enable damage to all hit enemy
    }



    public void ExecuteUpSkyLightAttack()
    {
        GetComponent<PlayerParticles>().PlayLightAttackVFX(true);
        CameraShake.instance.Shake(0.02f, 10f, 0, 10, new Vector2(Random.Range(-0.5f, 0.5f), 1f));

        float halfAngle = angle / 2; // angle above and below the centerline of the attack cone
        float deltaAngle = halfAngle / rayCount * 2; // change in degree between each ray

        Vector2 orig = transform.position;
#if DEBUG        
        Debug.DrawLine(orig, lastMousePos * range + orig, Color.red, 1.0f);
#endif

        for (int i = 1; i <= rayCount / 2; i++)
        {
            CastDamageRay(orig, deltaAngle * i, lastMousePos);
            CastDamageRay(orig, deltaAngle * -i, lastMousePos);
        }
        AudioManager.Instance.SFXBranch.PlaySFXTrack("AirAttack");
        hits.Clear(); // re-enable damage to all hit enemy
    }



    public void EndSkyLightAttack()
    {
        /*
        if (playerStateMachine.lightAttackQueued) return;
        playerStateMachine.SetLightAttackRecoveryState(false);
        */
        /*
        isSkyLightAttackDashing = false;
        GetComponent<Move>().PlayerGo();
        rb.velocity *= 0.6f;
        GetComponent<Move>().ApplyKnockback(rb.velocity.normalized, rb.velocity.magnitude, true);
        */
        GetComponent<Animator>().SetTrigger("OPT");
    }



    public void StopSkyLightAttack()
    {
        //GetComponent<Move>().PlayerGo();
    }





    // UTILITIES

    /// <summary>
    /// Cast a ray in some deviation in angle from the centerRay
    /// Check for 
    /// </summary>
    private void CastDamageRay(Vector2 orig, float angle, Vector3 centerRay)
    {
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        RaycastHit2D[] hit = Physics2D.RaycastAll(orig, rot * centerRay, range, attackMask);

#if DEBUG
        Debug.DrawLine(orig, rot * centerRay * range + transform.position, Color.blue, 1.0f);
#endif

        foreach (RaycastHit2D h in hit)
        {
            // check if hit is valid
            if (h.collider == null || !hits.Add(h.collider.gameObject.GetInstanceID()))
            {
                return;
            }

            foreach (IDamageable damageable in h.collider.gameObject.GetComponents<IDamageable>())
            {
                damageable.Damage(lightDamage, gameObject);
            }
        }
    }



    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
