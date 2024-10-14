using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// General Enemy AI to govern enemy behavior
/// </summary>
public class EnemyStateManager : MonoBehaviour
{
    public enum States
    {
        Idle,
        Approach,
        Attack
    }
    public Transform meleeHit;
    
    public float speed;
    public float aggroRange = 10.0f;
    public float attackCD = 2.0f;

    protected Transform player;
    protected States curState;
    protected Rigidbody rb;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        curState = States.Idle;
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        switch (curState)
        {
            case States.Idle:
                IdleBehavior();
                break;
            case States.Approach:
                ApproachBehavior();
                break;
            case States.Attack:
                AttackBehavior();
                break;
        }
        attackCD -= Time.deltaTime;
        UpdateState();
    }

    /// <summary>
    /// Update Enemy's behavior state based on line of sight and attack range
    /// </summary>
    protected void UpdateState()
    {
        if (!HasLineOfSight())
        {
            curState = States.Idle;
        }
        else if (InAttackRange())
        {
            curState = States.Attack;
        }
        else
        {
            curState = States.Approach;
        }
    }

    // Enemy behavior when Idle.
    protected virtual void IdleBehavior() { }
    // Enemy behavior when Approaching Player. Base implementation provided.
    protected virtual void ApproachBehavior() 
    {
        if (player.position.x - transform.position.x < 0)
        {
            Flip(false);
            rb.velocity = Vector3.left * speed;
        }
        else
        {
            Flip(true);
            rb.velocity = Vector3.right * speed;
        }
    }
    // Enemy behavior when Attacking.
    protected virtual void AttackBehavior() { }

    /// <summary>
    /// Project a ray in front of the Enemy to detect any Player in aggroRange
    /// In combat, will lock onto the player and maintain aggro for extended aggroRange
    /// Will loose aggro if there is Environment blocking line of sight
    /// </summary>
    /// <returns> If there is a Player in Enemy line of sight </returns>
    protected bool HasLineOfSight()
    {
        Vector3 dir = transform.TransformDirection(Vector3.right);
        float maxDistance = aggroRange;

        if (curState != States.Idle)
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
    protected virtual bool InAttackRange() { return false; }

    /// <summary>
    /// Flip the Enemy object across the Y-axis
    /// </summary>
    /// <param name="isFlipped"> Enemy's current orientation </param>
    protected void Flip(bool isFlipped)
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

    protected void OnDrawGizmos()
    {
        if (!HasLineOfSight()) 
        {
            Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * aggroRange);
        }
        else
        {
            Gizmos.DrawRay(transform.position, player.position - transform.position);
        }
        Gizmos.DrawCube(meleeHit.transform.position, meleeHit.transform.lossyScale);
    }
}
