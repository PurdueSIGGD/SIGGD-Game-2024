using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public enum States
    {
        Idle,
        Approach,
        Attack
    }
    public Transform meleeHit;
    public Transform player;
    public float speed;
    public float aggroRange = 10.0f;
    public float attackCD = 2.0f;

    protected States curState;
    protected Rigidbody rb;

    void Awake()
    {
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

    protected void UpdateState()
    {
        if (!HasLineOfSight())
        {
            curState = States.Idle;
        }
        else if (InMeleeRange())
        {
            curState = States.Attack;
        }
        else
        {
            curState = States.Approach;
            return;
        }
    }

    protected virtual void IdleBehavior() { }

    protected virtual void ApproachBehavior() 
    {
        if (player.position.x - transform.position.x < 0)
            rb.velocity = Vector3.left * speed;
        else
            rb.velocity = Vector3.right * speed;
    }

    protected virtual void AttackBehavior() { }

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

    protected bool InMeleeRange()
    {
        Collider[] c = Physics.OverlapBox(meleeHit.transform.position, meleeHit.transform.lossyScale / 2, meleeHit.transform.rotation, LayerMask.GetMask("Player"));
        return c.Length > 0;
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
