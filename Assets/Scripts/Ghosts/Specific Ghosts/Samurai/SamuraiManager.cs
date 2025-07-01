using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SamuraiManager : GhostManager, ISelectable
{
    public DamageContext heavyDamageContext;
    public ActionContext onDashContext;
    public ActionContext onParryContext;

    [HideInInspector] public bool selected;
    [HideInInspector] public WrathHeavyAttack basic; // the heavy attack ability
    [HideInInspector] public SamuraiRetribution special; // the special ability

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

    protected override void Start()
    {
        base.Start();
        int[] points = SaveManager.data.ghostSkillPts[identityName];
        Skill[] skills = GetComponent<SkillTree>().GetAllSkills();
        for (int i = 0; i < skills.Length; i++)
        {
            Debug.Log("skill Points from: " + skills[i].GetPoints());
            for (int j = 0; j < skills[i].GetPoints(); j++)
            {
                Debug.Log("Removed skillPoints from: " + skills[i]);
                GetComponent<SkillTree>().RemoveSkillPoint(skills[i]);
            }
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
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
}
