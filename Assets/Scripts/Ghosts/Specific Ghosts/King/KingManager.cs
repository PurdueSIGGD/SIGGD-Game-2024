using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KingManager : GhostManager, ISelectable
{
    [SerializeField] public DamageContext shieldBreakDamage;
    [SerializeField] public DamageContext specialDamage;
    [SerializeField] public GameObject shieldCircleVFX;
    [SerializeField] public GameObject shieldExplosionVFX;
    [SerializeField] public GameObject specialExplosionVFX;

    public float currentShieldHealth;
    public float endShieldHealth;
    public bool selected;

    public ActionContext specialContext;

    [HideInInspector] public KingBasic basic;
    [HideInInspector] public KingSpecial special;
    [HideInInspector] public bool recompenceAvaliable = false;
    [Header("Thrown Shield Used by Recompence Skill")]
    public GameObject thrownShield;
    [HideInInspector] public bool hasShield; // will be toggled false if King throws shield

    [HideInInspector] public bool isInvincibilityActive;
    [HideInInspector] private float invincibilityDuration;
    [HideInInspector] private Sprite invincibilityIcon = null;

    private PlayerStateMachine psm;
    private string identityName;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        int[] points = SaveManager.data.ghostSkillPts[identityName];
        Skill[] skills = GetComponent<SkillTree>().GetAllSkills();
        for (int i = 0; i < skills.Length; i++)
        {
            for (int j = 0; j < points[i]; j++)
            {
                GetComponent<SkillTree>().RemoveSkillPoint(skills[i]);
            }
        }

        shieldBreakDamage.damage = stats.ComputeValue("Shield Break Damage");
        specialDamage.damage = stats.ComputeValue("Special Damage");

        currentShieldHealth = stats.ComputeValue("Shield Max Health");
        //currentShieldHealth += gameObject.GetComponent<RulersResilience>().GetExtraShieldHealth();
            
        endShieldHealth = 0f;
        selected = false;

        hasShield = true;
        psm = PlayerID.instance.GetComponent<PlayerStateMachine>();

        isInvincibilityActive = false;
        invincibilityDuration = 0f;

        initializeSpecialEnergy();

        StartCoroutine(LateStart());
    }

    protected IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.5f);
        setSpecialReady(currentSpecialEnergy >= stats.ComputeValue("Special Energy Cost"));
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

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += OnDamageSpecialEnergyGain;
        GameplayEventHolder.OnDamageFilter.Add(InvincibilityFilter);
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= OnDamageSpecialEnergyGain;
        GameplayEventHolder.OnDamageFilter.Remove(InvincibilityFilter);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        rechargeShieldHealth();
        UpdateInvincibility();
    }

    private void rechargeShieldHealth()
    {
        if ((basic != null && basic.isShielding) || currentShieldHealth >= stats.ComputeValue("Shield Max Health") || !hasShield)
        {
            //psm.OffCooldown("c_basic");
            return;
        }
        currentShieldHealth = Mathf.Clamp((currentShieldHealth + (stats.ComputeValue("Shield Health Regeneration Rate") * Time.deltaTime)), 0f, stats.ComputeValue("Shield Max Health"));
        //psm.OnCooldown("c_basic");
    }

    public void TakeShieldDamage(float damage)
    {
        addSpecialEnergy(damage * stats.ComputeValue("Shield Damage Special Energy Generation Multiplier"));
        GetComponent<DivineSmite>().OnTakeDamage(damage);
    }

    public override void Select(GameObject player)
    {
        Debug.Log("KING SELECTED");
        selected = true;

        if (PlayerID.instance.GetComponent<HeavyAttack>()) Destroy(PlayerID.instance.GetComponent<HeavyAttack>());
        basic = PlayerID.instance.AddComponent<KingBasic>();
        basic.manager = this;

        special = PlayerID.instance.AddComponent<KingSpecial>();
        special.manager = this;

        base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        selected = false;
        if (basic) Destroy(basic);
        if (!PlayerID.instance.GetComponent<HeavyAttack>()) PlayerID.instance.AddComponent<HeavyAttack>();

        special?.endSpecial(true, true);
        if (special) Destroy(special);

        EndInvincibility();

        PlayerParticles.instance.StopGhostBadBuff();
        PlayerParticles.instance.StopGhostEmpowered();

        base.DeSelect(player);
    }





    private void initializeSpecialEnergy()
    {
        LevelSwitching levelSwitchingScript = FindFirstObjectByType<LevelSwitching>();
        if (!SceneManager.GetActiveScene().name.Equals(levelSwitchingScript.GetHomeWorld()))
        {
            setSpecialEnergy(SaveManager.data.aegis.specialEnergy);
        }
        else
        {
            resetSpecialEnergy();
        }
    }

    public void resetSpecialEnergy()
    {
        currentSpecialEnergy = 0f;
        SaveManager.data.aegis.specialEnergy = currentSpecialEnergy;
        setSpecialReady(false);
    }

    public void setSpecialEnergy(float energy)
    {
        currentSpecialEnergy = energy;
        currentSpecialEnergy = Mathf.Min(currentSpecialEnergy, stats.ComputeValue("Special Energy Cost"));
        SaveManager.data.aegis.specialEnergy = currentSpecialEnergy;
        setSpecialReady(currentSpecialEnergy >= stats.ComputeValue("Special Energy Cost"));
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
        if (context.actionTypes.Contains(ActionType.SPECIAL_ABILITY)) return;

        float maxHealth = victimHealth.GetStats().ComputeValue("Max Health");
        float percentHealthDamaged = context.damage / maxHealth;
        Debug.Log("Current Energy: " + currentSpecialEnergy + "  |  Earned Energy: " + (specialEnergyPool.energyPool * percentHealthDamaged));
        addSpecialEnergy(specialEnergyPool.energyPool * percentHealthDamaged);
    }





    private void UpdateInvincibility()
    {
        if (isInvincibilityActive && invincibilityDuration > 0f) invincibilityDuration -= Time.deltaTime;
        if (isInvincibilityActive && invincibilityDuration <= 0f) EndInvincibility();
    }

    public void StartInvincibility(float duration, Sprite icon)
    {
        isInvincibilityActive = true;
        invincibilityDuration = Mathf.Max(duration, invincibilityDuration);
        invincibilityIcon = icon;
        PlayerParticles.instance.PlayGhostGoodBuff(GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor, 1f, 1f);
    }

    public void EndInvincibility()
    {
        if (!isInvincibilityActive) return;
        isInvincibilityActive = false;
        invincibilityDuration = 0f;
        PlayerParticles.instance.StopGhostGoodBuff();
    }
    
    public void InvincibilityFilter(ref DamageContext context)
    {
        if (context.victim.CompareTag("Player") && isInvincibilityActive)
        {
            context.damage = 0f;
            context.icon = null;
            DamageNumberManager.instance.PlayMessage(this.gameObject, 0f, invincibilityIcon, "Invincible!", GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor);

            // VFX
            CameraShake.instance.Shake(0.2f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
            GameObject surfaceExplosion = Instantiate(shieldExplosionVFX, transform);
            surfaceExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(2f, GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor);

            // SFX
            AudioManager.Instance.SFXBranch.GetSFXTrack("Aegis-Shield On Damage").SetPitch(1f, 1f);
            AudioManager.Instance.SFXBranch.PlaySFXTrack("Aegis-Shield On Damage");
        }
    }
}
