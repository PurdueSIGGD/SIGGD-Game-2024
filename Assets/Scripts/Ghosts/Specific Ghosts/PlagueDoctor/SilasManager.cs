using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SilasManager : GhostManager
{
    [SerializeField] public GameObject blightPotion;
    [SerializeField] public GameObject blightMinibomb;
    [SerializeField] public DamageContext bombDamage;
    [SerializeField] public DamageContext miniBombDamage;
    [SerializeField] public GameObject blightDebuff;
    [SerializeField] public DamageContext blightDamage;
    [SerializeField] public GameObject bombExplosionVFX;

    [SerializeField] public GameObject basicIngredientPickup;
    [SerializeField] public HealingContext basicHealing;

    [HideInInspector] public PlagueDoctorSpecial special;
    [HideInInspector] public int specialCharges;
    [HideInInspector] private bool isCooldownActive = false;

    [HideInInspector] public PlagueDocApothecary basic;
    [HideInInspector] public int ingredientsCollected = 0;
    [HideInInspector] public bool healReady = false;
    [HideInInspector] public bool autoCollectIngredients = false;

    [HideInInspector] public bool isSelected = false;

    private LevelSwitching levelSwitchingScript;

    [SerializeField] string identityName;



    private void OnEnable()
    {
        GameplayEventHolder.OnDeath += DropOnKill;
        GameplayEventHolder.OnDeath += OnKillVoiceLines;
        GameplayEventHolder.OnDamageDealt += OnDamageSpecialEnergyGain;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDeath -= DropOnKill;
        GameplayEventHolder.OnDeath -= OnKillVoiceLines;
        GameplayEventHolder.OnDamageDealt -= OnDamageSpecialEnergyGain;
    }



    void Awake()
    {
        identityName = name;

        if (identityName.Contains("(Clone)"))
        {
            identityName = identityName.Replace("(Clone)", "");
        }

        //if (!SaveManager.data.ghostSkillPts.ContainsKey(identityName))
        //{
        //    SaveManager.data.ghostSkillPts.Add(identityName, new int[7]);
        //}

        //if (!SaveManager.data.ghostLevel.ContainsKey(identityName))
        //{
        //    SaveManager.data.ghostLevel.Add(identityName, 0);
        //}

    }



    protected override void Start()
    {
        base.Start();
        bombDamage.damage = stats.ComputeValue("Special Bomb Damage");
        miniBombDamage.damage = stats.ComputeValue("Special Minibomb Damage");
        blightDamage.damage = 1f;
        //specialCharges = Mathf.FloorToInt(stats.ComputeValue("Special Max Charges"));
        basicHealing.healing = stats.ComputeValue("Basic Healing");

        // Apply saved ingredients
        levelSwitchingScript = FindFirstObjectByType<LevelSwitching>();
        if (SceneManager.GetActiveScene().name.Equals(levelSwitchingScript.GetHomeWorld()))
        {
            SetIngredientsCollected(0);
        }
        ingredientsCollected = SaveManager.data.silas.ingredientsCollected;

        // Skill points save manager init
        int[] points = SaveManager.data.ghostSkillPts[identityName];
        Skill[] skills = GetComponent<SkillTree>().GetAllSkills();
        for (int i = 0; i < skills.Length; i++)
        {
            for (int j = 0; j < points[i]; j++)
            {
                GetComponent<SkillTree>().RemoveSkillPoint(skills[i]);
            }
        }

        initializeSpecialEnergy();
    }



    protected override void Update()
    {
        // Apothecary ready tracker
        if (ingredientsCollected >= stats.ComputeValue("Basic Ingredient Cost"))
        {
            if (basic != null) PlayerID.instance.GetComponent<PlayerStateMachine>().OnCooldown("heal_ready");
            healReady = true;
        }
        if (ingredientsCollected < stats.ComputeValue("Basic Ingredient Cost"))
        {
            if (basic != null) PlayerID.instance.GetComponent<PlayerStateMachine>().OffCooldown("heal_ready");
            healReady = false;
        }

        /*
        // Special cooldown/charge cycler
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
        */

        base.Update();
    }



    public override void Select(GameObject player)
    {
        Debug.Log("SILAS SELECTED!");

        special = PlayerID.instance.AddComponent<PlagueDoctorSpecial>();
        special.manager = this;
        basic = PlayerID.instance.AddComponent<PlagueDocApothecary>();
        basic.manager = this;
        isSelected = true;

        base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        if (special) Destroy(special);
        if (basic) Destroy(basic);
        isSelected = false;

        // Remove Self-medicated Buff
        SelfMedicated selfMedicated = GetComponent<SelfMedicated>();
        if (selfMedicated.isBuffed)
        {
            selfMedicated.RemoveBuff();
        }

        base.DeSelect(player);
    }

    public void FightEnd()
    {
        autoCollectIngredients = true;
    }





    private void initializeSpecialEnergy()
    {
        LevelSwitching levelSwitchingScript = FindFirstObjectByType<LevelSwitching>();
        if (!SceneManager.GetActiveScene().name.Equals(levelSwitchingScript.GetHomeWorld()))
        {
            setSpecialEnergy(SaveManager.data.silas.specialEnergy, false);
            setSpecialCharges(SaveManager.data.silas.specialCharges);
        }
        else
        {
            resetSpecialEnergy();
            setSpecialCharges(0);
        }
    }

    public void resetSpecialEnergy()
    {
        currentSpecialEnergy = 0f;
        SaveManager.data.silas.specialEnergy = currentSpecialEnergy;
        setSpecialReady(specialCharges > 0f);
    }
    
    public void setSpecialEnergy(float energy)
    {
        setSpecialEnergy(energy, true);
    }

    public void setSpecialEnergy(float energy, bool cycleCharges)
    {
        currentSpecialEnergy = energy;
        currentSpecialEnergy = Mathf.Min(currentSpecialEnergy, stats.ComputeValue("Special Energy Cost"));
        SaveManager.data.silas.specialEnergy = currentSpecialEnergy;
        if (currentSpecialEnergy >= stats.ComputeValue("Special Energy Cost") && cycleCharges)
        {
            addSpecialCharge();
            if (specialCharges < stats.ComputeValue("Special Max Charges")) resetSpecialEnergy();
        }
        setSpecialReady(specialCharges > 0f);
    }

    public void addSpecialEnergy(float energy)
    {
        setSpecialEnergy(getSpecialEnergy() + energy);
    }

    public float getSpecialEnergy()
    {
        return currentSpecialEnergy;
    }

    private void OnDamageSpecialEnergyGain(DamageContext context)
    {
        SpecialEnergyPool specialEnergyPool = context.victim.GetComponent<SpecialEnergyPool>();
        Health victimHealth = context.victim.GetComponent<Health>();
        if (specialEnergyPool == null || victimHealth == null) return;
        if (context.attacker != PlayerID.instance.gameObject) return;
        //if (context.actionTypes.Contains(ActionType.SPECIAL_ABILITY)) return;

        float maxHealth = victimHealth.GetStats().ComputeValue("Max Health");
        float percentHealthDamaged = context.damage / maxHealth;
        Debug.Log("Current Energy: " + currentSpecialEnergy + "  |  Earned Energy: " + (specialEnergyPool.energyPool * percentHealthDamaged));
        addSpecialEnergy(specialEnergyPool.energyPool * percentHealthDamaged);
    }

    public void addSpecialCharge()
    {
        specialCharges = Mathf.Min(Mathf.FloorToInt(stats.ComputeValue("Special Max Charges")), (specialCharges + 1));
        SaveManager.data.silas.specialCharges = specialCharges;
        setSpecialReady(specialCharges > 0f);
    }

    public void consumeSpecialCharge()
    {
        specialCharges = Mathf.Max(0, (specialCharges - 1));
        SaveManager.data.silas.specialCharges = specialCharges;
        setSpecialReady(specialCharges > 0f);
    }

    public void setSpecialCharges(int charges)
    {
        specialCharges = charges; //Mathf.Clamp(charges, 0, Mathf.FloorToInt(stats.ComputeValue("Special Max Charges")));
        SaveManager.data.silas.specialCharges = specialCharges;
        setSpecialReady(specialCharges > 0f);
    }





    public void SetIngredientsCollected(int ingredientsCollected)
    {
        this.ingredientsCollected = ingredientsCollected;
        SaveManager.data.silas.ingredientsCollected = ingredientsCollected;
    }



    public void DropIngredient(Vector3 position)
    {
        Vector3 dropDir = (new Vector3(Random.Range(-1f, 1f), 1f, 0f)).normalized;
        GameObject ammoPickup = Instantiate(basicIngredientPickup, position, Quaternion.identity);
        ammoPickup.GetComponent<PlagueDocIngredientPickup>().InitializeIngredientPickup(this, dropDir * 10f);
    }



    public void DropOnKill(DamageContext context)
    {
        if (healReady) return;

        // Drop ingredient
        if (context.victim.CompareTag("Enemy") && context.victim.GetComponent<DropTable>() != null &&
            Random.Range(0f, 100f) <= stats.ComputeValue("Basic Ingredient Drop Chance"))
        {
            DropIngredient(context.victim.transform.position);
        }

        // Drop extra ingredient
        if (context.victim.CompareTag("Enemy") &&
            Random.Range(0f, 100f) <= stats.ComputeValue("Basic Bonus Ingredient Drop Chance"))
        {
            DropIngredient(context.victim.transform.position);
        }
    }

    private void OnKillVoiceLines(DamageContext context)
    {
        if (context.victim.CompareTag("Enemy") && context.victim.GetComponentInChildren<BlightDebuff>() != null && isSelected)
        {
            AudioManager.Instance.VABranch.PlayVATrack("Silas-PlagueDoc Blight On Kill");
            if (GetComponent<Epidemic>().pointIndex > 0) AudioManager.Instance.VABranch.PlayVATrack("Silas-PlagueDoc Epidemic");
        }
    }
}
