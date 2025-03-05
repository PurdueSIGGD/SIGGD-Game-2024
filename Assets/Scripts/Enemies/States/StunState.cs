using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A state identical to Idle state except Enemy can only exit state
/// after a set amount of time.
/// Consider it as a state connected from "Any State".
/// </summary>
public class StunState : MonoBehaviour, IEnemyStates
{
    float stunDuration = 0.5f; // default stun duration, used for hit-stuns

    // TODO remove
    private SpriteRenderer sr;

    public void EnterState(EnemyStateManager enemy)
    {
        StartCoroutine(StunCoroutine(enemy, stunDuration));

        // TODO remove
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.red;
    }

    public void EnterState(EnemyStateManager enemy, float duration)
    {
        StartCoroutine(StunCoroutine(enemy, duration));

        // TODO remove
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.red;
    }

    private IEnumerator StunCoroutine(EnemyStateManager enemy, float duration)
    {
        Debug.Log(enemy.name + " stunned! stun test");
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        enemy.pool.idle.Play(enemy.animator);
        yield return new WaitForSeconds(duration);
        enemy.SwitchState(enemy.AggroState);
        Debug.Log(enemy.name + " recovered! stun test");
    }

    public void UpdateState(EnemyStateManager enemy) { /* do nothing */ }
}
