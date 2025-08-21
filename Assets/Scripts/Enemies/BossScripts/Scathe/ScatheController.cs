using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatheController : BossController
{

    ScatheManager enemyStateManager;
    Rigidbody2D rb;
    Animator anim;
    [SerializeField] float deathTimer;

    public void Start()
    {
        base.Start();
        enemyStateManager = GetComponent<ScatheManager>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    public override void DefeatSequence()
    {
        base.DefeatSequence();
        EnableInvincibility();
        rb.velocity = Vector2.zero;

        // delete all currently fielded attacks
        List<GameObject> currentAttacks = new(enemyStateManager.GetActiveAttacks());
        for (int i = 0; i < currentAttacks.Count; i++)
        {
            Destroy(currentAttacks[i]);
        }
        enemyStateManager.enabled = false;
        anim.SetTrigger("dead");
        StartCoroutine(DefeatCoroutine());
    }
    IEnumerator DefeatCoroutine()
    {
        print("Blight and might may break my soul... but time shall never claim me...");
        yield return new WaitForSeconds(deathTimer);
        Destroy(gameObject);
        EndBossRoom();
    }
}
