using System;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

/// <summary>
/// General Enemy AI to govern enemy behavior
/// </summary>
public class EnemyStateManager : MonoBehaviour
{
    public IEnemyStates IdleState = new IdleState();
    public IEnemyStates AggroState = new AggroState();
    public IEnemyStates BusyState = new BusyState();
    public IEnemyStates MoveState = new MoveState();
    public StunState StunState = new StunState();

    [HideInInspector] public StatManager stats; // Enemy stats component
    [HideInInspector] public ActionPool pool; // A pool of attacks to randomly choose from
    [HideInInspector] public Animator animator;

    [SerializeField] public bool isFlyer;
    [SerializeField] protected float aggroRange; // Range for detecting players 
    protected IEnemyStates curState; // Enemy's current State, defaults to idle
    protected Transform player;
    protected Rigidbody2D rb;

    public bool isBeingKnockedBack;
    protected float currentKnockbackDurationTime;
    [SerializeField] protected bool grounded;

    protected virtual void Awake()
    {
        player = PlayerID.instance.gameObject.transform;
        stats = GetComponent<StatManager>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pool = GetComponent<ActionPool>();
        isBeingKnockedBack = false;
        
        SwitchState(IdleState);
    }

    protected void FixedUpdate()
    {
        if (StunState.isStunned)
        {
            StunState.UpdateState(this, Time.deltaTime);
        }
        else
        {
            curState.UpdateState(this);
        }
        Debug.Log(name + " " + curState);
        UpdateKnockbackTime();
        isGrounded();
    }

    /// <summary>
    /// Transitions to another Enemy State
    /// </summary>
    /// <param name="state"> The new Enemy State </param>
    public void SwitchState(IEnemyStates state)
    {
        curState = state;
        state.EnterState(this);
    }

    /// <summary>
    /// Ray cast for Player in aggroRange
    /// </summary>
    /// <param name="tracking"> If true, will actively track player for extended range </param>
    /// <returns> If there is a Player in Enemy line of sight </returns>
    public virtual bool HasLineOfSight(bool tracking)
    {
        Vector2 dir = transform.TransformDirection(Vector2.right);
        float maxDistance = aggroRange;

        if (tracking)
        {
            dir = player.position - transform.position;
            maxDistance = maxDistance * 1.5f;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxDistance, LayerMask.GetMask("Player", "Ground"));
        Debug.DrawRay(transform.position, dir);
        if (hit)
        {
            return hit.collider.gameObject.CompareTag("Player");
        }
        return false;
    }

    /// <summary>
    /// Stun the enemy for a set duration
    /// </summary>
    /// <param name="damageContext"> the damage context that resulted in the stun </param>
    /// <param name="duration"> the duration of the stun </param>
    public void Stun(DamageContext damageContext, float duration = 0f)
    {
        if (duration == 0f)
        {
            StunState.EnterState(this);
        }
        else
        {
            StunState.EnterState(this, duration);
        }
    }

    /// <summary>
    /// Knockback the enemy.
    /// </summary>
    /// <param name="direction">The direction of the knockback</param>
    /// <param name="knockbackStrength">The strength of the knockback</param>
    /// <param name="knockbackDuration">The movement lockout duration of the knockback</param>
    public void ApplyKnockback(Vector2 direction, float knockbackStrength, float movementLockoutDuration)
    {
        rb.AddForce(direction.normalized * knockbackStrength, ForceMode2D.Impulse);
        if (isBeingKnockedBack && movementLockoutDuration <= currentKnockbackDurationTime) return;
        isBeingKnockedBack = true;
        currentKnockbackDurationTime = movementLockoutDuration;
    }

    /// <summary>
    /// Knockback the enemy.
    /// </summary>
    /// <param name="direction">The direction of the knockback</param>
    /// <param name="knockbackStrength">The strength of the knockback</param>
    public void ApplyKnockback(Vector2 direction, float knockbackStrength)
    {
        ApplyKnockback(direction, knockbackStrength, 0.5f);
    }

    private void UpdateKnockbackTime()
    {
        if (!isBeingKnockedBack) return;
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.position + Vector3.up, Color.green);
        currentKnockbackDurationTime -= Time.fixedDeltaTime;
        if (currentKnockbackDurationTime <= 0f)
        {
            isBeingKnockedBack = false;
            currentKnockbackDurationTime = 0f;
        }
    }

    /// <summary>
    /// Returns true if the enemy is on the ground.
    /// </summary>
    /// <returns>Returns true if the enemy is on the ground.</returns>
    public bool isGrounded()
    {
        Debug.DrawRay(transform.position, Vector2.down, Color.blue);
        return grounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
    }

    /// <summary>
    /// Flip the Enemy object across the Y-axis
    /// </summary>
    /// <param name="isFlipped"> Enemy's current orientation </param>
    public void Flip(bool isFlipped)
    {
        if (isFlipped)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180f, 0);
    }

    /// <summary>
    /// Do damage to player if they are inside of trigger box
    /// </summary>
    /// <param name="pos">Position of the trigger box</param>
    /// <para name="width">X value of the lossyscale of the trigger box</para>
    /// <para name="height">Y value of the lossyscale of the trigger box</para>
    /// <param name="damageContext">Instance of damage context</param>
    protected bool GenerateDamageFrame(Vector2 pos, float width, float height, DamageContext damageContext, GameObject attacker /*float damage*/)
    {
#if DEBUG // Draw the damage box in the editor
        float hWidth = width / 2;
        float hHeight = height / 2;
        float duration = 0.1f;

        Debug.DrawLine(new Vector2(pos.x - hWidth, pos.y + hHeight), new Vector2(pos.x + hWidth, pos.y + hHeight), Color.white, duration); // draw top line
        Debug.DrawLine(new Vector2(pos.x - hWidth, pos.y + hHeight), new Vector2(pos.x - hWidth, pos.y - hHeight), Color.white, duration); // draw left line
        Debug.DrawLine(new Vector2(pos.x - hWidth, pos.y - hHeight), new Vector2(pos.x + hWidth, pos.y - hHeight), Color.white, duration); // draw bottom line
        Debug.DrawLine(new Vector2(pos.x + hWidth, pos.y + hHeight), new Vector2(pos.x + hWidth, pos.y - hHeight), Color.white, duration); // draw right line
#endif
        // Check for player to do damage
        Collider2D hit = Physics2D.OverlapBox(pos, new Vector2(width, height), 0f, LayerMask.GetMask("Player"));
        if (hit)
        {
            PlayerID.instance.GetComponent<PlayerStateMachine>().SetStun(0.2f);
            hit.GetComponent<Health>().Damage(damageContext, attacker);
        }
        return hit;
    }

    /// <summary>
    /// Do damage to player if they are inside of trigger circle
    /// </summary>
    /// <param name="pos">Position of the trigger box</param>
    /// <param name="radius">X value of the lossyscale of the trigger circle</param>
    /// <param name="damageContext">Instance of damage context</param>
    protected bool GenerateDamageFrame(Vector2 pos, float radius, DamageContext damageContext, GameObject attacker /*float damage*/)
    {
#if DEBUG // Draw the damage circle in the editor
        int segment = 180;
        float duration = 0.2f;

        float angleDiv = 360f / segment;
        Vector2 p1 = new Vector2(pos.x + radius, pos.y);
        Vector2 p2;

        for (int i = 0; i < segment; i++)
        {
            float angle = angleDiv * i * Mathf.Deg2Rad;
            p2 = new Vector2(pos.x + Mathf.Cos(angle) * radius, pos.y + Mathf.Sin(angle) * radius);

            Debug.DrawLine(p1, p2, Color.white, duration);
            p1 = p2;
        }
#endif
        // Check for player to do damage
        Collider2D hit = Physics2D.OverlapCircle(pos, radius, LayerMask.GetMask("Player"));
        if (hit)
        {
            PlayerID.instance.GetComponent<PlayerStateMachine>().SetStun(0.2f);
            hit.GetComponent<Health>().Damage(damageContext, attacker);
        }
        return hit;
    }

    /// <summary>
    /// Draws the Enemy's line of sight in editor
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector2.right) * aggroRange);
    }

    /// <summary>
    /// Please attach this to the end of the animations that we wish
    /// not to be interrupted by other behaviors (attack animations)
    /// Attach this to an animation event at the end of animation
    /// </summary>
    protected virtual void OnFinishAnimation()
    {
        BusyState.ExitState(this);
    }
}
