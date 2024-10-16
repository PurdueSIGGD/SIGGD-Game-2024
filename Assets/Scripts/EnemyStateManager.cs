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

    public float speed;
    public float aggroRange;
    public Transform player;
    public ActionPool pool;

    protected EnemyStates curState;
    protected Animator animator;
    protected Rigidbody rb;

    void Awake()
    {
        curState = IdleState;
        curState.EnterState(this);
        GenerateActionPool();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        curState.UpdateState(this);
    }

    /// <summary>
    /// Project a ray in front of the Enemy to detect any Player in aggroRange
    /// In combat, will lock onto the player and maintain aggro for extended aggroRange
    /// Will loose aggro if there is Environment blocking line of sight
    /// </summary>
    /// <returns> If there is a Player in Enemy line of sight </returns>
    public bool HasLineOfSight(bool tracking)
    {
        Vector3 dir = transform.TransformDirection(Vector3.right);
        float maxDistance = aggroRange;

        if (tracking)
        {
            dir = player.position - transform.position;
            maxDistance = maxDistance * 1.5f;
        }

        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, maxDistance, LayerMask.GetMask("Player", "Environment")))
        {
            return hit.collider.gameObject.CompareTag("Player");
        }
        return false;
    }

    /// <summary>
    /// Project an overlap check to detect if a Player is within attack range.
    /// </summary>
    /// <returns> If a player is within attack range </returns>
    protected virtual bool InAttackRange(Transform hitbox)
    {
        Collider[] c = Physics.OverlapBox(hitbox.position, hitbox.lossyScale / 2, hitbox.rotation, LayerMask.GetMask("Player"));
        return c.Length > 0;
    }
    public virtual bool InAttackRange() { return false; } 

    /// <summary>
    /// Flip the Enemy object across the Y-axis
    /// </summary>
    /// <param name="isFlipped"> Enemy's current orientation </param>
    public void Flip(bool isFlipped)
    {
        if (isFlipped)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }

    }

    protected virtual ActionPool GenerateActionPool() { return null; }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * aggroRange);
        Gizmos.DrawRay(transform.position, player.position - transform.position);
    }

    public void SwitchState(EnemyStates state)
    {
        curState = state;
        state.EnterState(this);
    }
}
