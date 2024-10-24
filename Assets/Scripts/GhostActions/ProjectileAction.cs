using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Special action: the player shoots a projectile forward
/// </summary>
public class ProjectileAction : ISpecialAction
{
    private GameObject ProjectilePrefab;
    private GameObject ProjectileSpawn;

    private float _fireRate = 0.5f;
    private float _canFire = -0.1f;
    public ProjectileAction() {
        ProjectilePrefab = Resources.Load<GameObject>("Projectile");
        if (ProjectilePrefab == null)
        {
            Debug.LogError("Projectile prefab not found");
        }
        ProjectileSpawn = GameObject.Find("Player/ProjectileSpawn");
        if (ProjectileSpawn == null)
        {
            Debug.LogError("projectilespawn not found");
        }
    }
    public void EnterParty(GameObject player) {

    } 

    public void  ExitParty (GameObject player)
    {

    }

    public void UpdateSpecial() {

    }
    public void OnSpecial() {
        if (Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            Object.Instantiate(ProjectilePrefab, ProjectileSpawn.transform.position, ProjectileSpawn.transform.rotation);
        }
    }
}