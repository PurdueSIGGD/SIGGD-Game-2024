using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy AI for Mage.
/// </summary>
public class MageFire : EnemyStateManager
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] GameObject lineOfSightTriggerBox;



    [Header("Basic Projectile")]
    [SerializeField] private GameObject projectile;
    private Vector3 throwPosition = Vector3.zero;

    public void StartBasic()
    {
        throwPosition = PlayerID.instance.transform.position;
        if (!IsCurrentTargetPlayer())
        {
            throwPosition = GetCurrentTarget().transform.position;
        }
    }

    public void FireProjectile()
    {
        Instantiate(projectile, (transform.position + (0.5f * Vector3.right)), transform.rotation).GetComponent<EnemyProjectile>().Init(gameObject, throwPosition);
    }



    public void Update()
    {
        // manually flip the mage to face the player
        if (player != null)
        {
            if (player.position.x - transform.position.x < 0)
            {
                Flip(false);
            }
            else
            {
                Flip(true);
            }
        }
    }
    public void FireFireBall()
    {
        MageFireBallAttack ball = Instantiate(fireballPrefab, transform.position, Quaternion.identity).GetComponent<MageFireBallAttack>();
        ball.Initialize(player.gameObject, this.gameObject);
    }
    /*
    public override bool HasLineOfSight(bool tracking)
    {
        // override L.O.S. calculation to be really super generous to the mage rather than require direct L.O.S.
        return Physics2D.OverlapCircle(lineOfSightTriggerBox.transform.position, lineOfSightTriggerBox.transform.lossyScale.x, LayerMask.GetMask("Player")) || base.HasLineOfSight(tracking);
    }
    */

    // Draws the Mage's attack range
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, lineOfSightTriggerBox.transform.lossyScale.x);
    }

}
