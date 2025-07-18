using Unity.VisualScripting;
using UnityEngine;

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

    [HideInInspector] public PoliceChiefBasic basic;
    [HideInInspector] public PoliceChiefSpecial special;

    [SerializeField] string identityName;

    void Awake()
    {

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
        basicDamage.damage = stats.ComputeValue("Basic Damage");
        specialDamage.damage = stats.ComputeValue("Special Damage");
        basicAmmo = Mathf.RoundToInt(stats.ComputeValue("Basic Starting Ammo"));

        int[] points = SaveManager.data.ghostSkillPts[identityName];
        Skill[] skills = GetComponent<SkillTree>().GetAllSkills();
        for (int i = 0; i < skills.Length; i++)
        {
            for (int j = 0; j < points[i]; j++)
            {
                GetComponent<SkillTree>().RemoveSkillPoint(skills[i]);
            }
        }
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

		base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        if (basic) Destroy(basic);
        //if (!PlayerID.instance.GetComponent<HeavyAttack>()) PlayerID.instance.AddComponent<HeavyAttack>();

        if (special) special.endSpecial(false, false);
        if (special) Destroy(special);

		base.DeSelect(player);
    }

}
