using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class EnemyStateManager : MonoBehaviour
{
    public enum States
    {
        Idle,
        Approach,
        Attack
    }
    public States curState = States.Idle;
    public bool patrolling = false;
    public float aggroRange = 10.0f;

    void FixedUpdate()
    {
        switch (curState)
        {
            case States.Idle:
                break;
            case States.Approach:
                break;
            case States.Attack:
                break;
        }
        
    }

    protected void UpdateState()
    {
        if (!HasLineOfSight())
        {
            curState = States.Idle;
        }
        // if player not in attack range, move towards them
        
        // if player in attack range, attack
    }

    protected void IdleBehavior()
    {
        if (patrolling)
        {
            // do patrol
        }
    }

    protected void ApproachBehavior()
    {

    }

    protected void AttackBehavior()
    {

    }

    protected bool HasLineOfSight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, aggroRange))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.green);

            if (hit.collider.gameObject.CompareTag("Player")) 
            {
                return true;
            }
        }
        return false;
    }
}
