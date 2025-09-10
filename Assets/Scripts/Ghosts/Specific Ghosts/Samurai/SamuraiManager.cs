using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SamuraiManager : GhostManager, ISelectable
{
    public DamageContext heavyDamageContext;
    public DamageContext specialMeleeParryContext;
    public ActionContext onDashContext;
    public ActionContext onParryContext;
    public GameObject parrySuccessVFX;

    [HideInInspector] public bool selected;
    [HideInInspector] public WrathHeavyAttack basic; // the heavy attack ability
    [HideInInspector] public SamuraiRetribution special; // the special ability

    [HideInInspector] public float wrathPercent = 0f;
    [HideInInspector] public float decayTimer = 0f;
    [HideInInspector] public bool startingToDecay = false;
    [HideInInspector] public bool decaying = false;
    [HideInInspector] public bool resetDecay = false;

    private RoninsResolve roninsResolve;

    [SerializeField] string identityName;

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
        //    SaveManager.data.ghostLevel.Add(identityName, 10);
        //}
    }

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += WrathOnDamage;
        GameplayEventHolder.OnDeath += OnKillVoiceline;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= WrathOnDamage;
        GameplayEventHolder.OnDeath -= OnKillVoiceline;
    }

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
        roninsResolve = GetComponent<RoninsResolve>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (startingToDecay)
        {
            decayTimer -= Time.deltaTime;
            if (decayTimer < 0.0f)
            {
                startingToDecay = false;
                decaying = true;
            }
        }
        float decayRate = stats.ComputeValue("Wrath Decay Rate");
        if (decaying && wrathPercent >= decayRate * Time.deltaTime)
        {
            wrathPercent -= decayRate * Time.deltaTime;
            roninsResolve.RemoveBoosts(wrathPercent);
        }
        else if (decaying)
        {
            wrathPercent = 0f;
            decaying = false;
            roninsResolve.RemoveBoosts(wrathPercent);
        }

        // Wrath VFX
        if (selected)
        {
            if (wrathPercent >= 1f) PlayerParticles.instance.PlayGhostEmpowered(GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor, wrathPercent, 1f);
            else if (wrathPercent > 0f) PlayerParticles.instance.PlayGhostEmpowered(GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor, wrathPercent, 1f);
            else PlayerParticles.instance.StopGhostEmpowered();
        }
    }

    public override void Select(GameObject player)
    {
        Debug.Log("Akihito SELECTED!");
        selected = true;
        if (PlayerID.instance.GetComponent<HeavyAttack>()) Destroy(PlayerID.instance.GetComponent<HeavyAttack>());
        basic = PlayerID.instance.AddComponent<WrathHeavyAttack>();
        basic.manager = this;

        special = PlayerID.instance.AddComponent<SamuraiRetribution>();
        special.manager = this;

        roninsResolve.ActivateBoosts();

        if (GetComponent<RelentlessFury>().buffStacks > 0) PlayerParticles.instance.PlayGhostBadBuff(GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor, 0.5f, 1f);

        base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        roninsResolve.DeactivateBoosts();

        selected = false;
        if (basic) Destroy(basic);
        if (!PlayerID.instance.GetComponent<HeavyAttack>()) PlayerID.instance.AddComponent<HeavyAttack>();

        if (special) Destroy(special);

        // VFX
        PlayerParticles.instance.StopGhostEmpowered();
        PlayerParticles.instance.StopGhostGoodBuff();
        PlayerParticles.instance.StopGhostBadBuff();

        base.DeSelect(player);
    }



    //The function gets called (via event) whenever something gets damaged in the scene
    public void WrathOnDamage(DamageContext context)
    {
        // Gain Wrath on damage dealt
        if (context.attacker == PlayerID.instance.gameObject && context.actionID != ActionID.SAMURAI_BASIC)
        {
            float wrathGained = stats.ComputeValue("Wrath Percent Gain Per Damage Dealt") * context.damage / 100f;
            wrathGained = GetComponent<Vengeance>().CalculateBoostedWrathGain(context, wrathGained);

            // SFX & VFX
            if (wrathPercent < 1f && wrathPercent + wrathGained >= 1f)
            {
                AudioManager.Instance.SFXBranch.PlaySFXTrack("Akihito-Wrath Max");
                if (selected)
                {
                    GameObject maxWrathPulse = Instantiate(parrySuccessVFX, PlayerID.instance.transform.position, Quaternion.identity);
                    maxWrathPulse.GetComponent<RingExplosionHandler>().playRingExplosion(2f, GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor);
                    AudioManager.Instance.VABranch.PlayVATrack("Akihito-Samurai Max Wrath");
                    if (roninsResolve.pointIndex > 0) AudioManager.Instance.VABranch.PlayVATrack("Akihito-Samurai Ronins Resolve");
                }
            }
            else
            {
                AudioManager.Instance.SFXBranch.GetSFXTrack("Akihito-Wrath Gained").SetPitch(wrathPercent, 1f);
                AudioManager.Instance.SFXBranch.PlaySFXTrack("Akihito-Wrath Gained");
            }

            wrathPercent = Mathf.Min(wrathPercent + wrathGained, 1f);
            decayTimer = stats.ComputeValue("Wrath Decay Buffer");
            startingToDecay = true;
            decaying = false;
            roninsResolve.AddBoosts(wrathPercent);
        }

        // Lose Wrath on damage taken
        else if (context.victim == PlayerID.instance.gameObject && context.damage > 0f)
        {
            float wrathLost = stats.ComputeValue("Wrath Percent Loss Per Damage Taken") * context.damage / 100f;

            // SFX
            if (wrathPercent > 0f)
            {
                AudioManager.Instance.SFXBranch.GetSFXTrack("Akihito-Wrath Lost").SetPitch(wrathPercent, 1f);
                AudioManager.Instance.SFXBranch.PlaySFXTrack("Akihito-Wrath Lost");
            }

            wrathPercent = Mathf.Max(wrathPercent - wrathLost, 0f);
            roninsResolve.RemoveBoosts(wrathPercent);
        }
    }


    private void OnKillVoiceline(DamageContext context)
    {
        if (context.attacker.CompareTag("Player") && context.actionID == ActionID.SAMURAI_BASIC)
        {
            AudioManager.Instance.VABranch.PlayVATrack("Akihito-Samurai Wrath On Kill");
        }

        if (context.attacker.CompareTag("Player") && selected && PlayerHealth.instance.MortallyWounded)
        {
            AudioManager.Instance.VABranch.PlayVATrack("Akihito-Samurai Unwavering Will");
        }

        if (context.attacker.CompareTag("Player") && selected && GetComponent<RelentlessFury>().buffStacks > 0)
        {
            AudioManager.Instance.VABranch.PlayVATrack("Akihito-Samurai Relentless Fury");
        }
    }
}
