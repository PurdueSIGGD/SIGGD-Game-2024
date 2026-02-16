using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpiritBomber : EnemyStateManager
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float bombCooldown; //Cooldown time for bombs
                                                 //  [SerializeField] private float bombRange; // range of when to throw bombs
    [SerializeField] protected Transform bombSpawn;
    [SerializeField] protected Transform bombShoot;

    protected Vector3 throwPosition = Vector3.zero;

    protected override void Start()
    {
        base.Start();
        GameObject playacol = GameObject.FindWithTag("Player");
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playacol.GetComponent<Collider2D>());
    }
    /* public void Update()
     {
             if (Time.time - lastBombTime > bombCooldown)
             {
                 ThrowBomb();
             }
     } */
    //initialize bomb

    protected void StartThrow()
    {
        throwPosition = PlayerID.instance.transform.position;
        if (!IsCurrentTargetPlayer())
        {
            throwPosition = GetCurrentTarget().transform.position;
        }
    }

    protected void ThrowSFX()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("EnemyThrow");
    }

    protected void ThrowBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, bombSpawn.position, Quaternion.identity);
        EnemyProjectile trackingProjectile = bomb.GetComponent<EnemyProjectile>();
        Transform targetTransform = PlayerID.instance.transform;
        if (!IsCurrentTargetPlayer())
        {
            targetTransform = GetCurrentTarget().transform;
        }
        trackingProjectile.Init(this.gameObject, throwPosition, targetTransform);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(bombSpawn.position, bombSpawn.lossyScale);
        Gizmos.DrawWireCube(bombShoot.position, bombShoot.lossyScale);
    }
}
