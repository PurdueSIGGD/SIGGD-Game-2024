using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A state identical to Idle state except Enemy can only exit state
/// after a set amount of time.
/// Consider it as a state connected from "Any State".
/// </summary>
public class StunState : IEnemyStates
{
    public bool isStunned;
    protected float stunDuration;

    public void EnterState(EnemyStateManager enemy)
    {
        isStunned = true;
        this.stunDuration = 0.5f; // default stun duration, used for hit-stuns
    }

    public void EnterState(EnemyStateManager enemy, float stunDuration)
    {
        isStunned = true;
        this.stunDuration = stunDuration;
    }

    public void UpdateState(EnemyStateManager enemy) { /* do nothing */ }
    
    public void UpdateState(EnemyStateManager enemy, float delta)
    {
        stunDuration -= delta;
        if (stunDuration <= 0.0f)
        {
            isStunned = false;
            enemy.SwitchState(enemy.AggroState);
            Debug.Log(enemy.name + " recovered! stun test");
        }
        else
        {
            // TODO remove
            Debug.Log(enemy.name + " stunned! stun test");

            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            enemy.pool.idle.Play(enemy.animator);
        }
    }
}
