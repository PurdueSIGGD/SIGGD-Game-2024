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
        EnterState(enemy, 0.5f);
    }

    public void EnterState(EnemyStateManager enemy, float stunDuration)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        Debug.Log(enemy.name + " stunned! stun test");
        isStunned = true;
        if (!enemy.isBeingKnockedBack && (enemy.isFlyer || enemy.isGrounded())) rb.velocity = new Vector2(0, rb.velocity.y);
        this.stunDuration = stunDuration;
        enemy.animator.speed = 0;
    }

    public void UpdateState(EnemyStateManager enemy, float delta)
    {
        stunDuration -= delta;
        if (stunDuration <= 0.0f)
        {
            isStunned = false;
            enemy.animator.speed = 1;
            enemy.SwitchState(enemy.AggroState);
            Debug.Log(enemy.name + " recovered from stun!");
        }
        else
        {
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if (!enemy.isBeingKnockedBack && (enemy.isFlyer || enemy.isGrounded())) rb.velocity = new Vector2(0, rb.velocity.y);
            //enemy.pool.idle.Play(enemy.animator);
        }
    }

    public void UpdateState(EnemyStateManager enemy) { /* do nothing */ }
}