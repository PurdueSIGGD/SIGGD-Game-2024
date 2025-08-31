using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DivineSmite : Skill
{
    private KingManager manager;
    public int pointIndex;
    private bool divineSmitePowered = false;
    [SerializeField] public int[] dmgNeeded = {0, 800, 600, 400, 200};
    [SerializeField] private float damageBoost = 2f;
    [SerializeField] public float knockbackBoost = 1.25f;
    [SerializeField] public float radiusBoost = 1.5f;

    public float damageProgress;

    private void Start()
    {
        manager = GetComponent<KingManager>();
        damageProgress = SaveManager.data.aegis.damageDealtTillSmite;
        if (damageProgress >= dmgNeeded[pointIndex])
        {
            divineSmitePowered = true;
            SaveManager.data.aegis.damageDealtTillSmite = dmgNeeded[pointIndex];
            damageProgress = SaveManager.data.aegis.damageDealtTillSmite;
        }
    }

    private void Update()
    {
        if (manager.selected && pointIndex > 0)
        {
            if (divineSmitePowered) PlayerParticles.instance.PlayGhostEmpowered(GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor, 1f, 1f);
            else PlayerParticles.instance.StopGhostEmpowered();
        }
    }

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageFilter.Add(OnDivineSmite);
        GameplayEventHolder.OnDamageDealt += DamageDealtCharge;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageFilter.Remove(OnDivineSmite);
        GameplayEventHolder.OnDamageDealt -= DamageDealtCharge;
    }

    public void OnTakeDamage(float dmg)
    {
        if (pointIndex > 0 && !divineSmitePowered)
        {
            SaveManager.data.aegis.damageDealtTillSmite += dmg;
            damageProgress = SaveManager.data.aegis.damageDealtTillSmite;
            if (damageProgress >= dmgNeeded[pointIndex])
            {
                AudioManager.Instance.SFXBranch.PlaySFXTrack("Aegis-Divine Smite");
                manager.setSpecialCooldown(0f);
                divineSmitePowered = true;
                SaveManager.data.aegis.damageDealtTillSmite = dmgNeeded[pointIndex];
                damageProgress = SaveManager.data.aegis.damageDealtTillSmite;
            }
        }
    }

    public bool isSpecialPowered()
    {
        return divineSmitePowered;
    }

    void OnDivineSmite(ref DamageContext context)
    {
        if (context.actionID == ActionID.KING_SPECIAL && divineSmitePowered && pointIndex > 0)
        {
            context.damage *= damageBoost;
            context.damageStrength = DamageStrength.HEAVY;
        }
    }

    private void DamageDealtCharge(DamageContext context)
    {
        if (context.actionID == ActionID.KING_SPECIAL && divineSmitePowered && pointIndex > 0)
        {
            StartCoroutine(SmiteEndCoroutine());
            return;
        }

        if (context.attacker != null && context.attacker.CompareTag("Player") && pointIndex > 0 && !divineSmitePowered)
        {
            SaveManager.data.aegis.damageDealtTillSmite += context.damage;
            damageProgress = SaveManager.data.aegis.damageDealtTillSmite;
            if (damageProgress >= dmgNeeded[pointIndex])
            {
                AudioManager.Instance.SFXBranch.PlaySFXTrack("Aegis-Divine Smite");
                manager.setSpecialCooldown(0f);
                divineSmitePowered = true;
                SaveManager.data.aegis.damageDealtTillSmite = dmgNeeded[pointIndex];
                damageProgress = SaveManager.data.aegis.damageDealtTillSmite;
            }
        }
    }


    private IEnumerator SmiteEndCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        divineSmitePowered = false;
        SaveManager.data.aegis.damageDealtTillSmite = 0;
        damageProgress = SaveManager.data.aegis.damageDealtTillSmite;
    }

    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void ClearPointsTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
    }
}
