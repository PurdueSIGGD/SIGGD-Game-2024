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

    void Awake()
    {
        stats = GetComponent<StatManager>();
        instance = this;
    }

    void Start()
    {
        currentHealth = stats.ComputeValue("Max Health");
        GameplayEventHolder.OnDamageDealt += PlayHurtExertion;
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
            // if light amount of damage
            if (context.damage <= 30)
            {
                AudioManager.Instance.VABranch.PlayVATrack(PartyManager.instance.selectedGhost + " Light Damage Taken");
            }

            // if heavy damage taken
            if (context.damage > 30)
            {
                AudioManager.Instance.VABranch.PlayVATrack(PartyManager.instance.selectedGhost + " Significant Damage Taken");
            }
        }
    }
}
