using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuffs : MonoBehaviour
{
    private StatManager stats;
    private ItemInventory inventory;

    [SerializeField] private ItemSO alacrity;
    [SerializeField] private int alacrityBuff;

    [SerializeField] private ItemSO attunement;
    [SerializeField] private int attunementBuff;

    [SerializeField] private ItemSO impendingDoom;
    [SerializeField] private int impendingDoomBuff;

    [SerializeField] private ItemSO relentlessPursuit;
    [SerializeField] private int relentlessPursuitBuff;

    [SerializeField] private ItemSO slipstream;
    [SerializeField] private int slipstreamBuff;

    [SerializeField] private ItemSO tailwind;
    [SerializeField] private int tailwindBuff;

    [SerializeField] private ItemSO uncannyReflexes;
    [SerializeField] private int uncannyReflexesBuff;

    [SerializeField] private ItemSO upRush;
    [SerializeField] private int upRushBuff;

    [SerializeField] private ItemSO windBreaker;
    [SerializeField] private int windBreakerBuff;



    [SerializeField] private ItemSO devastation;
    [SerializeField] private int devastationBuff;

    [SerializeField] private ItemSO executioner;
    [SerializeField] private int executionerBuff;

    [SerializeField] private ItemSO faustianBargain;
    [SerializeField] private int faustianBargainBuff;
    [SerializeField] private DamageContext faustianBargainDamage;

    [SerializeField] private ItemSO onslaught;
    [SerializeField] private int onslaughtBuff;

    [SerializeField] private ItemSO opportunist;
    [SerializeField] private int opportunistBuff;

    [SerializeField] private ItemSO paralyzer;
    [SerializeField] private int paralyzerBuff;

    [SerializeField] private ItemSO recklessRuin;
    [SerializeField] private int recklessRuinBuff;
    [SerializeField] private int recklessRuinCurse;

    [SerializeField] private ItemSO skirmisher;
    [SerializeField] private int skirmisherBuff;

    [SerializeField] private ItemSO vigor;
    [SerializeField] private int vigorBuff;



    [SerializeField] private ItemSO bane;
    [SerializeField] private int baneBuff;

    [SerializeField] private ItemSO battleScars;
    [SerializeField] private int battleScarsBuff;

    [SerializeField] private ItemSO bountifulHarvest;
    [SerializeField] private int bountifulHarvestBuff;

    [SerializeField] private ItemSO direStraits;
    [SerializeField] private int direStraitsBuff;

    [SerializeField] private ItemSO healingFactor;
    [SerializeField] private int healingFactorBuff;

    [SerializeField] private ItemSO lifeReap;

    [SerializeField] private ItemSO scourge;
    [SerializeField] private int scourgeBuff;

    [SerializeField] private ItemSO resolve;
    [SerializeField] private int resolveBuff;

    [SerializeField] private ItemSO vitality;
    [SerializeField] private int vitalityBuff;



    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<StatManager>();
        inventory = PersistentData.Instance.GetComponent<ItemInventory>();

        if (inventory.ownedItems.Contains(attunement)) ApplyItemBuff(attunement);
        if (inventory.ownedItems.Contains(impendingDoom)) ApplyItemBuff(impendingDoom);
        if (inventory.ownedItems.Contains(relentlessPursuit)) ApplyItemBuff(relentlessPursuit);
        if (inventory.ownedItems.Contains(slipstream)) ApplyItemBuff(slipstream);
        if (inventory.ownedItems.Contains(tailwind)) ApplyItemBuff(tailwind);
        if (inventory.ownedItems.Contains(uncannyReflexes)) ApplyItemBuff(uncannyReflexes);
        if (inventory.ownedItems.Contains(upRush)) ApplyItemBuff(upRush);
        if (inventory.ownedItems.Contains(windBreaker)) ApplyItemBuff(windBreaker);

        if (inventory.ownedItems.Contains(devastation)) ApplyItemBuff(devastation);
        if (inventory.ownedItems.Contains(executioner)) ApplyItemBuff(executioner);
        if (inventory.ownedItems.Contains(faustianBargain)) ApplyItemBuff(faustianBargain);
        if (inventory.ownedItems.Contains(onslaught)) ApplyItemBuff(onslaught);
        if (inventory.ownedItems.Contains(opportunist)) ApplyItemBuff(opportunist);
        if (inventory.ownedItems.Contains(paralyzer)) ApplyItemBuff(paralyzer);
        if (inventory.ownedItems.Contains(recklessRuin)) ApplyItemBuff(recklessRuin);
        if (inventory.ownedItems.Contains(skirmisher)) ApplyItemBuff(skirmisher);

        if (inventory.ownedItems.Contains(bane)) ApplyItemBuff(bane);
        if (inventory.ownedItems.Contains(battleScars)) ApplyItemBuff(battleScars);
        if (inventory.ownedItems.Contains(bountifulHarvest)) ApplyItemBuff(bountifulHarvest);
        if (inventory.ownedItems.Contains(direStraits)) ApplyItemBuff(direStraits);
        if (inventory.ownedItems.Contains(healingFactor)) ApplyItemBuff(healingFactor);
        if (inventory.ownedItems.Contains(scourge)) ApplyItemBuff(scourge);
        if (inventory.ownedItems.Contains(resolve)) ApplyItemBuff(resolve);
        if (inventory.ownedItems.Contains(vitality)) ApplyItemBuff(vitality);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipItem(ItemSO item)
    {
        if (item.displayName.Equals("Spark of Alacrity"))
        {
            PersistentData.Instance.GetComponent<SpiritTracker>().AddRunSpirits(Spirit.SpiritType.Blue, alacrityBuff);
        }



        else if (item.displayName.Equals("Spark of Vigor"))
        {
            PersistentData.Instance.GetComponent<SpiritTracker>().AddRunSpirits(Spirit.SpiritType.Red, vigorBuff);
        }

        else if (item.displayName.Equals("Faustian Bargain"))
        {
            Health playerHealth = GetComponent<Health>();
            faustianBargainDamage.damage = playerHealth.GetStats().ComputeValue("Max Health") * playerHealth.GetStats().ComputeValue("Mortal Wound Threshold");
            playerHealth.NoContextDamage(faustianBargainDamage, gameObject);
            ApplyItemBuff(item);
        }



        else if (item.displayName.Equals("Spark of Resolve"))
        {
            PersistentData.Instance.GetComponent<SpiritTracker>().AddRunSpirits(Spirit.SpiritType.Yellow, resolveBuff);
        }

        else if (item.displayName.Equals("Life Reap"))
        {
            Health playerHealth = PlayerID.instance.GetComponent<Health>();
            playerHealth.currentHealth = Mathf.Min((playerHealth.currentHealth + (playerHealth.GetStats().ComputeValue("Max Health") * playerHealth.GetStats().ComputeValue("Mortal Wound Threshold"))),
                                                   playerHealth.GetStats().ComputeValue("Max Health"));
        }

        else if (item.displayName.Equals("Vitality"))
        {
            ApplyItemBuff(item);
            GetComponent<Health>().currentHealth += vitalityBuff;
        }



        else
        {
            ApplyItemBuff(item);
        }
    }

    private void ApplyItemBuff(ItemSO item)
    {
        if (item.displayName.Equals("Attunement"))
        {
            stats.ModifyStat("Cooldown Speed Boost", attunementBuff);
        }

        if (item.displayName.Equals("Impending Doom"))
        {
            stats.ModifyStat("Heavy Charge Up Time", -impendingDoomBuff);
        }

        if (item.displayName.Equals("Relentless Pursuit"))
        {
            stats.ModifyStat("Max Running Speed", relentlessPursuitBuff);
            stats.ModifyStat("Running Accel.", relentlessPursuitBuff);
            stats.ModifyStat("Max Glide Speed", relentlessPursuitBuff);
            stats.ModifyStat("Glide Accel.", relentlessPursuitBuff);
        }

        if (item.displayName.Equals("Slipstream"))
        {
            stats.ModifyStat("Max Glide Speed", slipstreamBuff);
            stats.ModifyStat("Glide Accel.", slipstreamBuff);
        }

        if (item.displayName.Equals("Tailwind"))
        {
            stats.ModifyStat("Max Dash Distance", tailwindBuff);
        }

        if (item.displayName.Equals("Uncanny Reflexes"))
        {
            stats.ModifyStat("Dodge Chance", uncannyReflexesBuff * 10);
        }

        if (item.displayName.Equals("Uprush"))
        {
            stats.ModifyStat("Jump Force", upRushBuff);
        }

        if (item.displayName.Equals("Windbreaker"))
        {
            stats.ModifyStat("Air Attack Dash Speed", windBreakerBuff);
        }



        if (item.displayName.Equals("Devastation"))
        {
            stats.ModifyStat("Crit Damage Boost", devastationBuff);
        }

        if (item.displayName.Equals("Executioner"))
        {
            stats.ModifyStat("Heavy Attack Damage Boost", executionerBuff);
        }

        if (item.displayName.Equals("Faustian Bargain"))
        {
            stats.ModifyStat("General Attack Damage Boost", faustianBargainBuff);
        }

        if (item.displayName.Equals("Onslaught"))
        {
            stats.ModifyStat("Crit Chance", onslaughtBuff);
        }

        if (item.displayName.Equals("Opportunist"))
        {
            stats.ModifyStat("Stun Damage Boost", opportunistBuff);
        }

        if (item.displayName.Equals("Paralyzer"))
        {
            stats.ModifyStat("Stun Duration Boost", paralyzerBuff);
        }

        if (item.displayName.Equals("Reckless Ruin"))
        {
            stats.ModifyStat("Crit Chance", recklessRuinBuff);
            stats.ModifyStat("General Attack Damage Boost", recklessRuinCurse);
        }

        if (item.displayName.Equals("Skirmisher"))
        {
            stats.ModifyStat("Light Attack Damage Boost", skirmisherBuff);
        }



        if (item.displayName.Equals("Bane"))
        {
            stats.ModifyStat("Projectile/Area Damage Resistance", baneBuff);
        }

        if (item.displayName.Equals("Battle Scars"))
        {
            int battleScarsHealth = battleScarsBuff * PersistentData.Instance.GetComponent<ItemInventory>().battleScarsStacks++;
            stats.ModifyStat("Max Health", battleScarsHealth);
            GetComponent<Health>().currentHealth += battleScarsBuff;
        }

        if (item.displayName.Equals("Bountiful Harvest"))
        {
            stats.ModifyStat("Spirit Drop Rate Boost", bountifulHarvestBuff);
        }

        if (item.displayName.Equals("Dire Straits"))
        {
            stats.ModifyStat("Critical Health Damage Resistance", direStraitsBuff);
        }

        if (item.displayName.Equals("Healing Factor"))
        {
            stats.ModifyStat("Healing Boost", healingFactorBuff);
        }

        if (item.displayName.Equals("Scourge"))
        {
            stats.ModifyStat("Melee Damage Resistance", scourgeBuff);
        }

        if (item.displayName.Equals("Vitality"))
        {
            stats.ModifyStat("Max Health", vitalityBuff);
        }
    }
}
