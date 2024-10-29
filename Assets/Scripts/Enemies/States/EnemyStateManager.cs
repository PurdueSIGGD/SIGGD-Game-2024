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

    public float speed;
    public float aggroRange;
    public ActionPool pool;
    
    public Transform player;
    public Animator animator;

    protected Rigidbody2D rb;
    protected EnemyStates curState;
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

    public void SwitchState(EnemyStates state)
    {
        curState = state;
        state.EnterState(this);
    }

    /// <summary>
    /// Project a ray in front of the Enemy to detect any Player in aggroRange
    /// In combat, will lock onto the player and maintain aggro for extended aggroRange
    /// Will loose aggro if there is Environment blocking line of sight
    /// </summary>
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

    protected virtual ActionPool GenerateActionPool() { return null; }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector2.right) * aggroRange);
    }
}
