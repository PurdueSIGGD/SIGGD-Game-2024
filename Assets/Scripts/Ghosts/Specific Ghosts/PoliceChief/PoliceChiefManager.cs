using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoliceChiefManager : GhostManager, ISelectable
{
    [SerializeField] public DamageContext basicDamage;
    [SerializeField] public DamageContext specialDamage;
    [SerializeField] public GameObject basicAmmoPickup;
    [SerializeField] public GameObject basicShot;
    [SerializeField] public GameObject basicTracerVFX;
    [SerializeField] public GameObject basicImpactExplosionVFX;
    [SerializeField] public GameObject specialShot;
    [SerializeField] public GameObject specialTracerVFX;
    [SerializeField] public GameObject specialImpactExplosionVFX;
    [SerializeField] public ActionContext sidearmActionContext;
    [SerializeField] public ActionContext policeChiefRailgun;

    [HideInInspector] public int basicAmmo;
    [HideInInspector] public bool autoRecoverAmmo = false;

    [HideInInspector] public PoliceChiefBasic basic;
    [HideInInspector] public PoliceChiefSpecial special;

    //[SerializeField] string identityName;

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
        GameplayEventHolder.OnDeath += OnKillVoiceLines;
        GameplayEventHolder.OnDamageDealt += OnDamageSpecialEnergyGain;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDeath -= OnKillVoiceLines;
        GameplayEventHolder.OnDamageDealt -= OnDamageSpecialEnergyGain;
    }

    protected override void Start()
    {
        base.Start();
        basicDamage.damage = stats.ComputeValue("Basic Damage");
        specialDamage.damage = stats.ComputeValue("Special Damage");
        basicAmmo = Mathf.RoundToInt(stats.ComputeValue("Basic Starting Ammo"));

        Debug.Log("Current Energy: " + getSpecialEnergy() + "  |  Energy Cost: " + stats.ComputeValue("Special Energy Cost"));

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

        StartCoroutine(LateStart());
    }

    protected IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.5f);
        setSpecialReady(currentSpecialEnergy >= stats.ComputeValue("Special Energy Cost"));
    }

    protected override void Update()
    {
        base.Update();
    }

    // ISelectable interface in use
    public override void Select(GameObject player)
    {
        Debug.Log("NORTH SELECTED!");

        //if (PlayerID.instance.GetComponent<HeavyAttack>()) Destroy(PlayerID.instance.GetComponent<HeavyAttack>());
        basic = PlayerID.instance.AddComponent<PoliceChiefBasic>();
        basic.manager = this;

        special = PlayerID.instance.AddComponent<PoliceChiefSpecial>();
        special.manager = this;

        if (GetComponent<PoliceChiefLethalForce>().shotEmpowered)
        {
            AudioManager.Instance.SFXBranch.PlaySFXTrack("North-Sidearm Primed Loop");
            PlayerID.instance.GetComponent<PlayerParticles>().PlayGhostEmpowered(GetComponent<GhostIdentity>().GetCharacterInfo().whiteColor, 1f, 1f);
        }

		base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        if (basic) Destroy(basic);
        //if (!PlayerID.instance.GetComponent<HeavyAttack>()) PlayerID.instance.AddComponent<HeavyAttack>();

        if (special) special.endSpecial(false, false);
        if (special) Destroy(special);

        if (GetComponent<PoliceChiefLethalForce>().shotEmpowered)
        {
            AudioManager.Instance.SFXBranch.StopSFXTrack("North-Sidearm Primed Loop");
            PlayerID.instance.GetComponent<PlayerParticles>().StopGhostEmpowered();
        }

        base.DeSelect(player);
    }

    public void FightEnd()
    {
        autoRecoverAmmo = true;
    }





    private void initializeSpecialEnergy()
    {
        LevelSwitching levelSwitchingScript = FindFirstObjectByType<LevelSwitching>();
        if (!SceneManager.GetActiveScene().name.Equals(levelSwitchingScript.GetHomeWorld()))
        {
            setSpecialEnergy(SaveManager.data.north.specialEnergy);
        }
        else
        {
            resetSpecialEnergy();
        }
    }

    public void resetSpecialEnergy()
    {
        currentSpecialEnergy = 0f;
        SaveManager.data.north.specialEnergy = currentSpecialEnergy;
        setSpecialReady(false);
    }

    public void setSpecialEnergy(float energy)
    {
        currentSpecialEnergy = energy;
        currentSpecialEnergy = Mathf.Min(currentSpecialEnergy, stats.ComputeValue("Special Energy Cost"));
        SaveManager.data.north.specialEnergy = currentSpecialEnergy;
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





    private void OnKillVoiceLines(DamageContext context)
    {
        if (context.victim.CompareTag("Enemy") && context.actionID == ActionID.POLICE_CHIEF_SPECIAL)
        {
            AudioManager.Instance.VABranch.PlayVATrack("North-Police_Chief Railgun On Kill");
            return;
        }

        if (context.victim.CompareTag("Enemy") && context.actionID == ActionID.POLICE_CHIEF_BASIC)
        {
            AudioManager.Instance.VABranch.PlayVATrack("North-Police_Chief Sidearm On Kill");
            return;
        }
    }
    

}
