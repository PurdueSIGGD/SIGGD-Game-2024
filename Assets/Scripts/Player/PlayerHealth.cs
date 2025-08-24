using UnityEngine;

public class PlayerHealth : Health
{
    public static PlayerHealth instance;

    public bool Wounded { get; private set; }
    public bool MortallyWounded { get; private set; }

    private float thresholdOne;
    private float thresholdTwo;
    private float maxHealth;
    private float healthProportion;

    [SerializeField] private int baseDodgeChance;
    [SerializeField] private Sprite dodgeIcon;
    [SerializeField] private Sprite tempoIcon;
    [SerializeField] private Sprite selfMedicatedIcon;
    [SerializeField] private CharacterSO orionCharacterInfo;
    [SerializeField] private GameObject dodgeVFX;

    [HideInInspector] public IdolPassive evaTempo;
    [HideInInspector] public SelfMedicated silasSelfMedicated;

    void Awake()
    {
        stats = GetComponent<StatManager>();
        instance = this;
    }

    void Start()
    {
        currentHealth = stats.ComputeValue("Max Health");
        stats.ModifyStat("Dodge Chance", 900);
        stats.ModifyStat("Dodge Chance", baseDodgeChance * 10);
    }

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += PlayHurtExertion;
        GameplayEventHolder.OnDamageFilter.Add(CheckDodgeChance);
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= PlayHurtExertion;
        GameplayEventHolder.OnDamageFilter.Remove(CheckDodgeChance);
    }

    void Update()
    {
        maxHealth = stats.ComputeValue("Max Health");
        thresholdOne = stats.ComputeValue("Wounded Threshold");
        thresholdTwo = stats.ComputeValue("Mortal Wound Threshold");

        healthProportion = currentHealth / maxHealth;

        // Mortal Wound Check
        if (healthProportion <= thresholdTwo)
        {
            Wounded = false;
            MortallyWounded = true;
        }

        // Wound Check
        else if (healthProportion <= thresholdOne)
        {
            Wounded = true;
            MortallyWounded = false;
        }

        // Healthy Check
        else
        {
            Wounded = false;
            MortallyWounded = false;
        }
    }



    public override float Heal(HealingContext context, GameObject healer)
    {
        if (MortallyWounded)
        {
            context.healing = Mathf.Clamp(context.healing, 0, thresholdTwo * maxHealth - currentHealth);
            if (currentHealth == (thresholdTwo * maxHealth))
            {
                context.healing = 0f;
            }
        }
        else if (Wounded)
        {
            context.healing = Mathf.Clamp(context.healing, 0, thresholdOne * maxHealth - currentHealth);
            if (currentHealth == (thresholdOne * maxHealth))
            {
                context.healing = 0f;
            }
        }
        return base.Heal(context, healer);
    }

    /// <summary>
    /// Function for audio playing hurt sounds from losing health.
    /// </summary>
    /// <param name="context"></param>
    private void PlayHurtExertion(DamageContext context)
    {
        if (context.victim.CompareTag("Player"))
        {
            if (context.damage <= 0f)
            {
                return;
            }

            // if light amount of damage
            if (!context.isCriticalHit)
            {
                AudioManager.Instance.VABranch.PlayVATrack(PartyManager.instance.selectedGhost + " Light Damage Taken");
                return;
            }

            // if heavy damage taken
            if (context.isCriticalHit)
            {
                AudioManager.Instance.VABranch.PlayVATrack(PartyManager.instance.selectedGhost + " Significant Damage Taken");
            }

            AudioManager.Instance.SFXBranch.PlaySFXTrack("PlayerTakeDamageSFX");

            // play mortal wound sfx, if appplicable
            if ((currentHealth + context.damage) / maxHealth > thresholdTwo && currentHealth / maxHealth <= thresholdTwo)
            {
                AudioManager.Instance.SFXBranch.PlaySFXTrack("MortalWound2ndSFX");
            }
            else if ((currentHealth + context.damage) / maxHealth > thresholdOne && currentHealth / maxHealth <= thresholdOne)
            {
                AudioManager.Instance.SFXBranch.PlaySFXTrack("MortalWound1stSFX");
            }
        }
    }



    // Dodge Chance Handler
    private void CheckDodgeChance(ref DamageContext context)
    {
        if (!context.victim.CompareTag("Player")) return;
        if (context.damageTypes.Contains(DamageType.STATUS) || context.damageTypes.Contains(DamageType.ENVIRONMENTAL)) return;

        if (Random.Range(1000f, 2000f) > stats.ComputeValue("Dodge Chance")) return;
        context.damage = 0f;

        // Eva Tempo Dodge Effect
        GhostIdentity selectedGhost = GetComponent<PartyManager>().GetSelectedGhost();
        if (evaTempo != null && evaTempo.tempoStacks > 0 && selectedGhost != null && selectedGhost.GetCharacterInfo().displayName.Equals("Eva"))
        {
            DamageNumberManager.instance.PlayMessage(gameObject, 0f, tempoIcon, "Dodged!", selectedGhost.GetCharacterInfo().highlightColor);
            GameObject tempoDodgePulseVFX = Instantiate(dodgeVFX, gameObject.transform);
            tempoDodgePulseVFX.GetComponent<RingExplosionHandler>().playRingExplosion(2f, selectedGhost.GetCharacterInfo().highlightColor);
            return;
        }

        // Silas Self-medicated Dodge Effect
        if (silasSelfMedicated != null && silasSelfMedicated.isBuffed && selectedGhost != null && selectedGhost.GetCharacterInfo().displayName.Equals("Silas"))
        {
            DamageNumberManager.instance.PlayMessage(gameObject, 0f, selfMedicatedIcon, "Dodged!", selectedGhost.GetCharacterInfo().highlightColor);
            GameObject selfMedicatedDodgePulseVFX = Instantiate(dodgeVFX, gameObject.transform);
            selfMedicatedDodgePulseVFX.GetComponent<RingExplosionHandler>().playRingExplosion(2f, selectedGhost.GetCharacterInfo().highlightColor);
            return;
        }

        // Standard Dodge Effect
        DamageNumberManager.instance.PlayMessage(gameObject, 0f, dodgeIcon, "Dodged!", orionCharacterInfo.highlightColor);
        GameObject dodgePulseVFX = Instantiate(dodgeVFX, gameObject.transform);
        dodgePulseVFX.GetComponent<RingExplosionHandler>().playRingExplosion(2f, orionCharacterInfo.highlightColor);
    }
}
