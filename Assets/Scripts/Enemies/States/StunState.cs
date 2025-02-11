using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A state identical to Idle state except Enemy can only exit state
/// after a set amount of time.
/// Consider it as a state connected from "Any State".
/// </summary>
public class StunState : EnemyStates
{
    float stunDuration = 0.5f; // default stun duration, used for hit-stuns

    public override void EnterState(EnemyStateManager enemy)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        enemy.pool.idle.Play(enemy.animator);
    }

    public void EnterState(EnemyStateManager enemy, float duration)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        enemy.pool.idle.Play(enemy.animator);
        stunDuration = duration;
    }
    
    public override void UpdateState(EnemyStateManager enemy)
    {
        // throw new UnityException("Obsolete method, please overload the method with delta time");
        // or
        enemy.SwitchState(enemy.AggroState); // immediately exits the stun
        stunDuration = 0.5f;
    }

    public void UpdateState(EnemyStateManager enemy, float deltaT)
    {
        stunDuration -= deltaT;
        if (stunDuration <= 0)
        {
            enemy.SwitchState(enemy.AggroState);
            stunDuration = 0.5f;
        }
    }
}
