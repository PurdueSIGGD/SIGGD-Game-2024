using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Special action: the player shoots a projectile forward
public class ProjectileAction : GhostAction
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
    public override void EnterSpecial() {

    } 

    public override void ExitSpecial() {

    }

    public override void UpdateSpecial() {

    }
    public override void OnSpecial(MonoBehaviour context) {
        if (Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            Object.Instantiate(ProjectilePrefab, ProjectileSpawn.transform.position, ProjectileSpawn.transform.rotation);
        }
    }
}