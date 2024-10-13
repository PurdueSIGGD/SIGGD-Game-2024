using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public enum States
    {
        Idle,
        Approach,
        Attack
    }
    public States curState = States.Idle;
    public Collider meleeHit;
    public LayerMask layerMask;

    public Rigidbody rb;
    public float speed;
    public float aggroRange = 10.0f;

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
        UpdateState();
    }

    protected void UpdateState()
    {
        if (!HasLineOfSight())
        {
            curState = States.Idle;
            return;
        }
        if (InMeleeRange())
        {
            curState = States.Attack;
            return;
        }
        else
        {
            curState = States.Approach;
            return;
        }
    }

    protected void IdleBehavior()
    {
        rb.velocity = Vector3.zero;
    }

    protected void ApproachBehavior()
    {
        rb.velocity = Vector3.right * speed;
    }

    protected void AttackBehavior()
    {
        rb.velocity = Vector3.zero;
    }

    protected bool HasLineOfSight()
    {
        return (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out RaycastHit hit, aggroRange, layerMask));
    }

    protected bool InMeleeRange()
    {
        Collider[] c = Physics.OverlapBox(meleeHit.transform.position, meleeHit.transform.lossyScale / 2, meleeHit.transform.rotation, layerMask);

        return c.Length > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(meleeHit.transform.position, meleeHit.transform.lossyScale);
    }
}
