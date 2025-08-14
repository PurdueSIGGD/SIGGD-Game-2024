using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SilasManager : GhostManager
{
    [SerializeField] public GameObject blightPotion;
    [SerializeField] public GameObject blightMinibomb;
    [SerializeField] public DamageContext bombDamage;
    [SerializeField] public DamageContext miniBombDamage;
    [SerializeField] public GameObject blightDebuff;
    [SerializeField] public DamageContext blightDamage;
    [SerializeField] public GameObject bombExplosionVFX;

    [SerializeField] public HealingContext basicHealing;

    [HideInInspector] public PlagueDoctorSpecial special;
    [HideInInspector] public int specialCharges;
    [HideInInspector] private bool isCooldownActive = false;



    protected override void Start()
    {
        base.Start();
        bombDamage.damage = stats.ComputeValue("Special Bomb Damage");
        miniBombDamage.damage = stats.ComputeValue("Special Minibomb Damage");
        blightDamage.damage = 1f;
        specialCharges = Mathf.FloorToInt(stats.ComputeValue("Special Max Charges"));
    }

    protected override void Update()
    {
        if (!isCooldownActive && specialCharges < stats.ComputeValue("Special Max Charges"))
        {
            isCooldownActive = true;
            startSpecialCooldown();
        }

        if (isCooldownActive && getSpecialCooldown() <= 0f)
        {
            isCooldownActive = false;
            specialCharges++;
        }

        base.Update();
    }



    public override void Select(GameObject player)
    {
        Debug.Log("SILAS SELECTED!");

        special = PlayerID.instance.AddComponent<PlagueDoctorSpecial>();
        special.manager = this;

        base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        if (special) Destroy(special);

        base.DeSelect(player);
    }
}
