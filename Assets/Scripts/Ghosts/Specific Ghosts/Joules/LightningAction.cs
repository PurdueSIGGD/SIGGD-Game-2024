using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Special action: a projectile shoots down in front of the player
/// </summary>
public class LightningAction : MonoBehaviour, IParty, ISelectable
{
    [SerializeField]
    private GameObject lightningPrefab;

    private GameObject projectileSpawn;

    private bool possessing;

    private float _fireRate = 1f;
    private float _canFire = -0.1f;
    public LightningAction()
    {
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
    public void EnterParty(GameObject player)
    {
        projectileSpawn = player;
    }

    public void ExitParty(GameObject player)
    {
        projectileSpawn = null;
    }
    public void Select(GameObject player)
    {
        possessing = true;
    }
    public void DeSelect(GameObject player)
    {
        possessing = false;
    }

    public void UpdateSpecial()
    {

    }
    public void OnSpecial()
    {
        if (possessing &&
            projectileSpawn != null &&
            Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            Object.Instantiate(lightningPrefab, projectileSpawn.transform.position + new Vector3(0, 5, 0), projectileSpawn.transform.rotation);
        }
    }
}