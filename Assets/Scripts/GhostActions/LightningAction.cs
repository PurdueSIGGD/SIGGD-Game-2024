using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Special action: a projectile shoots down in front of the player
/// </summary>
public class LightningAction : GhostAction
{
    private GameObject LightningPrefab;
    private GameObject ProjectileSpawn;

    private float _fireRate = 1f;
    private float _canFire = -0.1f;
    public LightningAction() {
        /*LightningPrefab = Resources.Load<GameObject>("Lightning");
        if (LightningPrefab == null)
        {
            Debug.LogError("Lightning prefab not found");
        }
        ProjectileSpawn = GameObject.Find("Player/ProjectileSpawn");
        if (ProjectileSpawn == null)
        {
            Debug.LogError("projectilespawn not found");
        }*/
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
            Object.Instantiate(LightningPrefab, ProjectileSpawn.transform.position + new Vector3(0, 5, 0), ProjectileSpawn.transform.rotation);
        }
    }
}