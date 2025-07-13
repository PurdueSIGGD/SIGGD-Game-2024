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

    [SerializeField] string identityName;

    void Awake()
    {
        if (identityName.Contains("(Clone)"))
        {
            identityName = identityName.Replace("(Clone)", "");
        }

        if (!SaveManager.data.ghostSkillPts.ContainsKey(identityName))
        {
            SaveManager.data.ghostSkillPts.Add(identityName, new int[7]);
        }

        if (!SaveManager.data.ghostLevel.ContainsKey(identityName))
        {
            SaveManager.data.ghostLevel.Add(identityName, 10);
        }
    }

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += WrathOnDamage;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= WrathOnDamage;
    }

    protected override void Start()
    {
        base.Start();
        int[] points = SaveManager.data.ghostSkillPts[identityName];
        Skill[] skills = GetComponent<SkillTree>().GetAllSkills();
        for (int i = 0; i < skills.Length; i++)
        {
            for (int j = 0; j < skills[i].GetPoints(); j++)
            {
                GetComponent<SkillTree>().RemoveSkillPoint(skills[i]);
            }
        }
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
        }
        else if (decaying)
        {
            wrathPercent = 0f;
            decaying = false;
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

        base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        selected = false;
        if (basic) Destroy(basic);
        if (!PlayerID.instance.GetComponent<HeavyAttack>()) PlayerID.instance.AddComponent<HeavyAttack>();

        if (special) Destroy(special);

        base.DeSelect(player);
    }



    //The function gets called (via event) whenever something gets damaged in the scene
    public void WrathOnDamage(DamageContext context)
    {
        if (context.attacker == PlayerID.instance.gameObject /*&& context.actionID != ActionID.SAMURAI_BASIC*/)
        {
            float wrathGained = stats.ComputeValue("Wrath Percent Gain Per Damage Dealt") * context.damage / 100f;
            wrathPercent = Mathf.Min(wrathPercent + wrathGained, 1f);
            decayTimer = stats.ComputeValue("Wrath Decay Buffer");
            startingToDecay = true;
            decaying = false;
        }
        else if (context.victim == PlayerID.instance.gameObject)
        {
            float wrathLost = stats.ComputeValue("Wrath Percent Loss Per Damage Taken") * context.damage / 100f;
            wrathPercent = Mathf.Max(wrathPercent - wrathLost, 0f);
        }
    }
}
