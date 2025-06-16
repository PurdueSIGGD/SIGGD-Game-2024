using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceChiefLodgedAmmoManager : MonoBehaviour
{

    private PoliceChiefManager manager;

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += LodgeAmmoOnHit;
        GameplayEventHolder.OnDamageDealt += DropAmmoOnMeleeHit;
        GameplayEventHolder.OnDeath += DropAmmoOnDeath;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= LodgeAmmoOnHit;
        GameplayEventHolder.OnDamageDealt -= DropAmmoOnMeleeHit;
        GameplayEventHolder.OnDeath -= DropAmmoOnDeath;
    }

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<PoliceChiefManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LodgeAmmoOnHit(DamageContext context)
    {
        if (!context.victim.CompareTag("Enemy")) return;
        if (context.actionID.Equals(ActionID.POLICE_CHIEF_BASIC))
        {
            LodgeAmmo(context, 1);
        }
    }

    private void DropAmmoOnMeleeHit(DamageContext context)
    {
        if (!context.victim.CompareTag("Enemy")) return;
        if (context.damageTypes.Contains(DamageType.MELEE))
        {
            DropAmmo(context, 1);
        }
    }

    private void DropAmmoOnDeath(DamageContext context)
    {
        if (!context.victim.CompareTag("Enemy")) return;
        DropAmmo(context, 9999);
    }

    public PoliceChiefLodgedAmmo LodgeAmmo(DamageContext context, int ammo)
    {
        PoliceChiefLodgedAmmo enemyLodgedAmmo = context.victim.GetComponent<PoliceChiefLodgedAmmo>();
        if (enemyLodgedAmmo == null)
        {
            enemyLodgedAmmo = context.victim.AddComponent<PoliceChiefLodgedAmmo>();
        }
        enemyLodgedAmmo.ammoLodged += ammo;
        return enemyLodgedAmmo;
    }

    public void DropAmmo(DamageContext context, int ammo)
    {
        PoliceChiefLodgedAmmo enemyLodgedAmmo = context.victim.GetComponent<PoliceChiefLodgedAmmo>();
        int ammoLodged = 1;
        if (enemyLodgedAmmo == null && context.actionID != ActionID.POLICE_CHIEF_BASIC) return;
        if (enemyLodgedAmmo != null)
        {
            ammoLodged = enemyLodgedAmmo.ammoLodged;
        }

        for (int i = 0; i < Mathf.Min(ammo, ammoLodged); i++)
        {
            Vector3 dropDir = (new Vector3(Random.Range(-1f, 1f), 1f, 0f)).normalized;
            GameObject ammoPickup = Instantiate(manager.basicAmmoPickup, context.victim.transform.position, Quaternion.identity);
            ammoPickup.GetComponent<PoliceChiefAmmoPickup>().InitializeAmmoPickup(manager, dropDir * 10f);
            enemyLodgedAmmo.ammoLodged = Mathf.Max(enemyLodgedAmmo.ammoLodged - 1, 0);
        }
    }
}
