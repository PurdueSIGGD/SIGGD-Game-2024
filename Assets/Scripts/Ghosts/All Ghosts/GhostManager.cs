using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostManager : MonoBehaviour, ISelectable, IStatList
{
    [SerializeField] public StatManager.Stat[] statList;
    [SerializeField] public RuntimeAnimatorController defaultController;
    [SerializeField] public RuntimeAnimatorController ghostController;

    protected Animator animator;
    protected PartyManager partyManager;
    protected StatManager stats;

    private float currentBasicCooldown = 0f;
    private bool basicReady = true;

    private float currentSpecialCooldown = 0f;
    private bool specialReady = true;


    private bool sacrificeReady = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = PlayerID.instance.GetComponent<Animator>();
        partyManager = PlayerID.instance.GetComponent<PartyManager>();
        stats = GetComponent<StatManager>();
        StartCoroutine(DelayedStartCoroutine());
    }

    private IEnumerator DelayedStartCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        int cooldownSpeedBoost = Mathf.FloorToInt(PlayerID.instance.GetComponent<PlayerBuffStats>().GetStats().ComputeValue("Cooldown Speed Boost") - 100f);
        //stats.ModifyStat("Basic Cooldown", -cooldownSpeedBoost);
        stats.ModifyStat("Special Cooldown", -cooldownSpeedBoost);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        updateSpecialCooldown();
        updateBasicCooldown();
    }

    public virtual void Select(GameObject player)
    {
        if (PlayerID.instance.GetComponent<Dash>()) Destroy(PlayerID.instance.GetComponent<Dash>());
        animator.runtimeAnimatorController = ghostController;
    }

    public virtual void DeSelect(GameObject player)
    {
        if (!PlayerID.instance.GetComponent<Dash>()) PlayerID.instance.AddComponent<Dash>();
        animator.runtimeAnimatorController = defaultController;
    }



    public void startSpecialCooldown()
    {
        currentSpecialCooldown = stats.ComputeValue("Special Cooldown");
        specialReady = false;
    }

    private void updateSpecialCooldown()
    {
        if (specialReady) return;
        currentSpecialCooldown -= Time.deltaTime;
        specialReady = (currentSpecialCooldown <= 0f);
        currentSpecialCooldown = Mathf.Max(currentSpecialCooldown, 0f);
    }

    public void setSpecialCooldown(float cooldown)
    {
        currentSpecialCooldown = cooldown;
        specialReady = (currentSpecialCooldown <= 0f);
        currentSpecialCooldown = Mathf.Max(currentSpecialCooldown, 0f);
    }

    public float getSpecialCooldown()
    {
        return currentSpecialCooldown;
    }



    public void startBasicCooldown()
    {
        currentBasicCooldown = stats.ComputeValue("Basic Cooldown");
        basicReady = false;
    }

    private void updateBasicCooldown()
    {
        if (basicReady) return;
        currentBasicCooldown -= Time.deltaTime;
        basicReady = (currentBasicCooldown <= 0f);
        currentBasicCooldown = Mathf.Max(currentBasicCooldown, 0f);
    }

    public void setBasicCooldown(float cooldown)
    {
        currentBasicCooldown = cooldown;
        basicReady = (currentBasicCooldown <= 0f);
        currentBasicCooldown = Mathf.Max(currentBasicCooldown, 0f);
    }

    public float getBasicCooldown()
    {
        return currentBasicCooldown;
    }

    //public void SetSacrificeReady(bool sacrificeReady)
    //{
    //    this.sacrificeReady = sacrificeReady;
    //}
    public bool GetSacrificeReady()
    {
        Sacrifice sac = GetComponent<Sacrifice>();
        if (sac && sac.GetPoints() > 0)
        {
            return true;
        }
        return false;
    }


    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }

    public StatManager GetStats()
    {
        return stats;
    }
}
