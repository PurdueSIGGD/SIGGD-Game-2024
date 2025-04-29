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

    public UnityEvent evaSelectedEvent;
    public UnityEvent evaDeselectedEvent;

    protected override void Start()
    {
        base.Start();
        passive = GetComponent<IdolPassive>();
        passive.manager = this;
    }

    protected override void Update()
    {
        base.Update();
        if (clones.Count > 0 && !clonesActive)
        {
            clonesActive = true;
        }
        if (clones.Count <= 0 && clonesActive)
        {
            clonesActive = false;
            startSpecialCooldown();
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

        base.Select(player);

        evaSelectedEvent?.Invoke();
    }

    public override void DeSelect(GameObject player)
    {
        passive.RemoveBuffOnSwap();

        if (PlayerID.instance.GetComponent<IdolSpecial>()) Destroy(PlayerID.instance.GetComponent<IdolSpecial>());
        base.DeSelect(player);
        evaDeselectedEvent?.Invoke();
    }
}