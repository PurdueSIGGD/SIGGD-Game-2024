using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KingManager : GhostManager, ISelectable
{
    [SerializeField] public DamageContext shieldBreakDamage;
    [SerializeField] public DamageContext specialDamage;
    [SerializeField] public GameObject shieldCircleVFX;
    [SerializeField] public GameObject shieldExplosionVFX;
    [SerializeField] public GameObject specialExplosionVFX;

    public float currentShieldHealth;
    public float endShieldHealth;

    [HideInInspector] public KingBasic basic;
    [HideInInspector] public KingSpecial special;

    private PlayerStateMachine psm;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        shieldBreakDamage.damage = stats.ComputeValue("Shield Break Damage");
        specialDamage.damage = stats.ComputeValue("Special Damage");
        currentShieldHealth = stats.ComputeValue("Shield Max Health");
        endShieldHealth = 0f;

        psm = PlayerID.instance.GetComponent<PlayerStateMachine>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        rechargeShieldHealth();
    }



    private void rechargeShieldHealth()
    {
        if ((basic != null && basic.isShielding) || currentShieldHealth >= stats.ComputeValue("Shield Max Health"))
        {
            psm.OffCooldown("c_basic");
            return;
        }
        currentShieldHealth = Mathf.Clamp((currentShieldHealth + (stats.ComputeValue("Shield Health Regeneration Rate") * Time.deltaTime)), 0f, stats.ComputeValue("Shield Max Health"));
        psm.OnCooldown("c_basic");
    }



    public override void Select(GameObject player)
    {
        Debug.Log("KING SELECTED");

        if (PlayerID.instance.GetComponent<HeavyAttack>()) Destroy(PlayerID.instance.GetComponent<HeavyAttack>());
        basic = PlayerID.instance.AddComponent<KingBasic>();
        basic.manager = this;

        special = PlayerID.instance.AddComponent<KingSpecial>();
        special.manager = this;

        base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        if (basic) Destroy(basic);
        if (!PlayerID.instance.GetComponent<HeavyAttack>()) PlayerID.instance.AddComponent<HeavyAttack>();

        special?.endSpecial(true, true);
        if (special) Destroy(special);

        base.DeSelect(player);
    }
}
