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
            context.healing = Mathf.Clamp(context.healing, 0, thresholdTwo - currentHealth);
        }
        else if (Wounded)
        {
            context.healing = Mathf.Clamp(context.healing, 0, thresholdOne - currentHealth);
        }
        return base.Heal(context, healer);
    }
}
