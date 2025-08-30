using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class IdolManager : GhostManager, ISelectable
{

    [HideInInspector] public IdolSpecial special;
    [HideInInspector] public IdolPassive passive;
    [SerializeField] public ActionContext onDashContext;
    [SerializeField] public ActionContext onSwapContext;

    [SerializeField] public GameObject holojumpTracerVFX;
    [SerializeField] public GameObject holojumpPulseVFX;
    [SerializeField] public GameObject tempoPulseVFX;
    [SerializeField] public GameObject explosionVFX;

    public bool active;
    [SerializeField] public GameObject idolClone;
    public List<GameObject> clones = new List<GameObject>(); // list of all active clones;
    public bool clonesActive = false;
    private bool cloneLowDuration = false;

    public UnityEvent evaSelectedEvent;
    public UnityEvent evaDeselectedEvent;

    private GhostIdentity identity;
    string identityName;

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
        passive = GetComponent<IdolPassive>();
        passive.manager = this;

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

        // Clone Activated
        if (clones.Count > 0 && !clonesActive)
        {
            clonesActive = true;
            PlayerID.instance.GetComponent<PlayerParticles>().PlayGhostGoodBuff(GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor, 1f, 1f);
        }

        // Clone Low Duration
        if (clones.Count > 0 && clones[0].GetComponent<IdolClone>().duration <= 3f && !cloneLowDuration)
        {
            cloneLowDuration = true;
            PlayerID.instance.GetComponent<PlayerParticles>().PlayGhostGoodBuff(GetComponent<GhostIdentity>().GetCharacterInfo().whiteColor, 1f, 1f);
        }

        // Clones Ended
        if (clones.Count <= 0 && clonesActive)
        {
            clonesActive = false;
            cloneLowDuration = false;
            PlayerID.instance.GetComponent<PlayerParticles>().StopGhostGoodBuff();
            startSpecialCooldown();

            // play audio, if has upgrade, choose from 1 random voice bank to play
            string chosenBank = passive.avaliableCloneLostVA[Random.Range(0, passive.avaliableCloneLostVA.Count)];
            AudioManager.Instance.VABranch.PlayVATrack(chosenBank);
        }
    }

    // ISelectable interface in use
    public override void Select(GameObject player)
    {
        Debug.Log("EVA SELECTED!");

        special = PlayerID.instance.AddComponent<IdolSpecial>();
        special.manager = this;
        special.idolClone = idolClone;
        passive.ApplyBuffOnSwap();

        if (clonesActive)
        {
            if (clones.Count > 0 && clones[0].GetComponent<IdolClone>().duration <= 3f)
            {
                PlayerID.instance.GetComponent<PlayerParticles>().PlayGhostGoodBuff(GetComponent<GhostIdentity>().GetCharacterInfo().whiteColor, 1f, 1f);
            }
            else
            {
                PlayerID.instance.GetComponent<PlayerParticles>().PlayGhostGoodBuff(GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor, 1f, 1f);
            }
        }

        base.Select(player);

        evaSelectedEvent?.Invoke();
    }

    public override void DeSelect(GameObject player)
    {
        passive.RemoveBuffOnSwap();

        if (clonesActive) PlayerID.instance.GetComponent<PlayerParticles>().StopGhostGoodBuff();

        if (PlayerID.instance.GetComponent<IdolSpecial>()) Destroy(PlayerID.instance.GetComponent<IdolSpecial>());
        base.DeSelect(player);
        evaDeselectedEvent?.Invoke();
    }
}