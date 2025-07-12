using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DivineSmite : Skill
{
    private KingManager manager;
    private int pointindex;
    private bool divineSmitePowered = false;
    [SerializeField] private int[] dmgNeeded = {800, 600, 400, 200};
    [SerializeField] private float damageBoost = 2f;

    private void Start()
    {
        manager = GetComponent<KingManager>();
        GameplayEventHolder.OnDamageFilter.Add(OnDivineSmite);
    }

    public void OnTakeDamage(float dmg)
    {
        if(GetPoints() > 0)
        {
            SaveManager.data.aegis.damageBlockTillSmite += dmg;
            if (SaveManager.data.aegis.damageBlockTillSmite > dmgNeeded[GetPoints() - 1])
            {
                divineSmitePowered = true;
                SaveManager.data.aegis.damageBlockTillSmite = 0;
            }
        }
    }

    public bool isSpecialPowered()
    {
        return divineSmitePowered;
    }

    void OnDivineSmite(ref DamageContext context)
    {
        if(context.actionID == ActionID.KING_SPECIAL)
        {
            context.damage *= damageBoost;
            divineSmitePowered = false;
        }
        if(context.attacker.CompareTag("Player") && GetPoints() > 0 && manager.selected)
        {
            SaveManager.data.aegis.damageDealtTillSmite += context.damage;
            if (SaveManager.data.aegis.damageDealtTillSmite > dmgNeeded[GetPoints() - 1])
            {
                divineSmitePowered = true;
                SaveManager.data.aegis.damageDealtTillSmite = 0;
            }
        }
    }

    public override void AddPointTrigger()
    {
        
    }

    public override void ClearPointsTrigger()
    {
        
    }

    public override void RemovePointTrigger()
    {
        
    }
}
