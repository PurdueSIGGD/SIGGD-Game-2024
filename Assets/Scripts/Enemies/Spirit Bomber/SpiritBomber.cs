using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritBomber : EnemyStateManager
{
    [SerializeField] private GameObject bombPrefab; 
    [SerializeField] private float bombCooldown; //Cooldown time for bombs
  //  [SerializeField] private float bombRange; // range of when to throw bombs
    [SerializeField] protected Transform bombSpawn;
    [SerializeField] protected Transform bombShoot;

    public void Start()
    {
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
    protected void ThrowBomb()
     {
         GameObject bomb = Instantiate(bombPrefab, bombSpawn.position, transform.rotation);
         TrackingProjectile trackingProjectile = bomb.GetComponent<TrackingProjectile>();

     }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(bombSpawn.position, bombSpawn.lossyScale);
        Gizmos.DrawWireCube(bombShoot.position, bombShoot.lossyScale);
    }
}
