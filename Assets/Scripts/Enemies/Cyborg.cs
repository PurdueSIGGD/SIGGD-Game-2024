using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Playables;

/// <summary>
/// Enemy AI for the Cyborg
/// </summary>
[SerializeField]
public class Cyborg : EnemyStateManager
{
    protected Transform meleeTrigger; // Area in which enemy will attempt to melee
    protected float meleeDamage;
    protected Transform tpBackTrigger; // Area in which enemy will attempt to teleport backward
    protected float spinDamage;
    protected Transform tpForwardTrigger; // Area in which enemy will attempt to teleport forward
    
    protected Transform stingerTrigger; // Area in which enemy will attempt to use stinger attack
    protected Transform stingerHitBox; // Area in which the stinger attack will do damage
    protected float stingerDamage;

    // Cyborg will draw actions greedily

    // Generate damage frame for melee slash attack
    protected void OnSlashEvent()
    {
        GenerateDamageFrame(meleeTrigger.position, meleeTrigger.lossyScale.x, meleeTrigger.lossyScale.y, meleeDamage);
    }

    // Teleports the cyborg backwards
    protected void OnTPBackEvent()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * -1, stingerTrigger.transform.localPosition.x, LayerMask.GetMask("Ground"));
        if (hit)
        {
            gameObject.transform.position = hit.point;
        }
        else
        {
            gameObject.transform.position -= new Vector3(stingerTrigger.transform.localPosition.x * transform.right.x, 0, 0);
        }
    }

    // Teleprts the cyborg forward
    protected void OnTPForwardEvent1()
    {
        Vector2 dest = new(player.position.x + meleeTrigger.lossyScale.x, player.position.y + 1);
        gameObject.transform.position = dest;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
    }

    // Generate damage frame for follow up attack after teleporting forward
    protected void OnTPForwardEvent2()
    {
        GenerateDamageFrame(tpBackTrigger.position, tpBackTrigger.lossyScale.x, tpBackTrigger.lossyScale.y, spinDamage);
    }

    // Generate damage frame for long range stinger attack
    protected void OnStingerEvent()
    {
        GenerateDamageFrame(stingerHitBox.position, stingerHitBox.lossyScale.x, stingerHitBox.lossyScale.y, stingerDamage);
    }

    protected override void OnFinishAnimation()
    {
        base.OnFinishAnimation();
        rb.gravityScale = 1; // TPForward will suspend the Cyborg in air, this resets that
    }


    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(meleeTrigger.position, meleeTrigger.lossyScale);
        Gizmos.DrawWireCube(tpBackTrigger.position, tpBackTrigger.lossyScale);
        Gizmos.DrawWireCube(tpForwardTrigger.position, tpForwardTrigger.lossyScale);
        Gizmos.DrawWireCube(stingerTrigger.position, stingerTrigger.lossyScale);
        Gizmos.DrawWireCube(stingerHitBox.position, stingerHitBox.lossyScale);

        // The max distance the cyborg will attempt to teleport back
        // Gizmos.DrawRay(transform.position, new Vector2(stingerTrigger.transform.localPosition.x * transform.right.x * -1, 0));
    }
}
