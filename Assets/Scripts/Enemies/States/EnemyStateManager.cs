using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// General Enemy AI to govern enemy behavior
/// </summary>
public class EnemyStateManager : MonoBehaviour
{
    public EnemyStates IdleState = new IdleState();
    public EnemyStates AggroState = new AggroState();
    public EnemyStates BusyState = new BusyState();
    public EnemyStates MoveState = new MoveState();

    public float speed; // Movement speed
    public float aggroRange; // Range for detecting players 
    
    public ActionPool pool; // A pool of attacks to randomly choose from
    public EnemyStates curState; // Enemy's current State, defaults to idle
    public Animator animator;

    protected Transform player;
    protected Rigidbody2D rb;
    
    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pool = GenerateActionPool();

        SwitchState(IdleState);
    }

    protected void FixedUpdate()
    {
        curState.UpdateState(this);
    }

    /// <summary>
    /// Transitions to another Enemy State
    /// </summary>
    /// <param name="state"> The new Enemy State </param>
    public void SwitchState(EnemyStates state)
    {
        curState = state;
        state.EnterState(this);
    }

    /// <summary>
    /// Ray cast for Player in aggroRange
    /// </summary>
    /// <param name="tracking"> If true, will actively track player for extended range </param>
    /// <returns> If there is a Player in Enemy line of sight </returns>
    public bool HasLineOfSight(bool tracking)
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
    /// Flip the Enemy object across the Y-axis
    /// </summary>
    /// <param ghostName="isFlipped"> Enemy's current orientation </param>
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
    /// <param name="gizmoTrigger">Transform component for drawing damage box</param>
    /// <param name="damage">Points of damage to deal</param>
    protected void GenerateDamageFrame(Transform gizmoTrigger, float damage)
    {
#if DEBUG // Draw the damage box in the editor 
        Vector2 center = gizmoTrigger.position;
        float hWidth = gizmoTrigger.lossyScale.x/2;
        float hHeight = gizmoTrigger.lossyScale.y/2;

        Debug.DrawLine(new Vector2(center.x - hWidth, center.y + hWidth), new Vector2(center.x + hWidth, center.y + hWidth)); // draw top line
        Debug.DrawLine(new Vector2(center.x - hWidth, center.y + hWidth), new Vector2(center.x - hWidth, center.y - hWidth)); // draw left line
        Debug.DrawLine(new Vector2(center.x - hWidth, center.y - hWidth), new Vector2(center.x + hWidth, center.y - hWidth)); // draw bottom line
        Debug.DrawLine(new Vector2(center.x + hWidth, center.y + hWidth), new Vector2(center.x + hWidth, center.y - hWidth)); // draw right line
#endif
        // Check for player to do damage
        Collider2D hit = Physics2D.OverlapBox(gizmoTrigger.position, gizmoTrigger.lossyScale, 0f, LayerMask.GetMask("Player"));
        if (hit)
        {
            hit.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    /// <summary>
    /// Produce a list off Actions which randomly generates the next action
    /// </summary>
    /// <returns></returns>
    protected virtual ActionPool GenerateActionPool() 
    { 
        return null; 
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
