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

        if (healthProportion <= thresholdTwo)
        {
            Wounded = false;
            MortallyWounded = true;
        }
        else if (healthProportion <= thresholdOne)
        {
            Wounded = true;
            MortallyWounded = false;
        }
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
        }
        else if (Wounded)
        {
            context.healing = Mathf.Clamp(context.healing, 0, thresholdOne * maxHealth - currentHealth);
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
            if (context.damage < 25f)
            {
                AudioManager.Instance.VABranch.PlayVATrack(PartyManager.instance.selectedGhost + " Light Damage Taken");
                return;
            }

            // if heavy damage taken
            if (context.damage >= 25f)
            {
                AudioManager.Instance.VABranch.PlayVATrack(PartyManager.instance.selectedGhost + " Significant Damage Taken");
            }
        }
    }

    private void CheckDodgeChance(ref DamageContext context)
    {
        if (!context.victim.CompareTag("Player")) return;

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

        // Standard Dodge Effect
        DamageNumberManager.instance.PlayMessage(gameObject, 0f, dodgeIcon, "Dodged!", orionCharacterInfo.highlightColor);
        GameObject dodgePulseVFX = Instantiate(dodgeVFX, gameObject.transform);
        dodgePulseVFX.GetComponent<RingExplosionHandler>().playRingExplosion(2f, orionCharacterInfo.highlightColor);
    }
}
