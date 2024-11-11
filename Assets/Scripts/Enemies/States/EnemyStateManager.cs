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
    public Transform player; // The player

    protected EnemyStates curState; // Enemy's current State, defaults to idle
    public Animator animator;
    protected Rigidbody2D rb;
    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pool = GenerateActionPool();

        SwitchState(IdleState);
    }

    void FixedUpdate()
    {
        pool.UpdateAllCD();
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
    protected void OnFinishAnimation()
    {
        BusyState.ExitState(this);
    }
}
