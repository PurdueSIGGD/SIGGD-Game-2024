using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostManager : MonoBehaviour, ISelectable, IStatList
{
    [SerializeField] public StatManager.Stat[] statList;
    [SerializeField] public RuntimeAnimatorController defaultController;
    [SerializeField] public RuntimeAnimatorController ghostController;

    protected Animator animator;
    protected PartyManager partyManager;
    protected StatManager stats;
    protected PlayerCursorManager playerCursorManager;

    private float currentBasicCooldown = 0f;
    private bool basicReady = true;

    private float currentSpecialCooldown = 0f;
    private bool specialReady = false;
    public float currentSpecialEnergy = 0f;

    private bool sacrificeReady = false;

    [SerializeField] public string identityName;

    /*
    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += OnDamageSpecialEnergyGain;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= OnDamageSpecialEnergyGain;
    }
    */

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = PlayerID.instance.GetComponent<Animator>();
        partyManager = PlayerID.instance.GetComponent<PartyManager>();
        stats = GetComponent<StatManager>();
        playerCursorManager = PlayerID.instance.GetComponent<PlayerCursorLink>().GetPlayerCursorManager();
        StartCoroutine(DelayedStartCoroutine());
    }

    private IEnumerator DelayedStartCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        int cooldownSpeedBoost = Mathf.FloorToInt(PlayerID.instance.GetComponent<PlayerBuffStats>().GetStats().ComputeValue("Cooldown Speed Boost") - 100f);
        //stats.ModifyStat("Basic Cooldown", -cooldownSpeedBoost);
        //stats.ModifyStat("Special Cooldown", -cooldownSpeedBoost);
        stats.ModifyStat("Special Energy Cost", -cooldownSpeedBoost);
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
        playerCursorManager.SetActiveGhost(GetComponent<GhostIdentity>());
    }

    public virtual void DeSelect(GameObject player)
    {
        if (!PlayerID.instance.GetComponent<Dash>()) PlayerID.instance.AddComponent<Dash>();
        animator.runtimeAnimatorController = defaultController;
        playerCursorManager.SetActiveGhost(null);
    }





    public void startSpecialCooldown()
    {
        currentSpecialCooldown = stats.ComputeValue("Special Cooldown");
        specialReady = false;
    }

    private void updateSpecialCooldown()
    {
        //if (specialReady) return;
        if (currentSpecialCooldown <= 0f) return;
        currentSpecialCooldown -= Time.deltaTime;
        //specialReady = (currentSpecialCooldown <= 0f);
        currentSpecialCooldown = Mathf.Max(currentSpecialCooldown, 0f);
    }

    public void setSpecialCooldown(float cooldown)
    {
        currentSpecialCooldown = cooldown;
        //specialReady = (currentSpecialCooldown <= 0f);
        currentSpecialCooldown = Mathf.Max(currentSpecialCooldown, 0f);
    }

    public float getSpecialCooldown()
    {
        return currentSpecialCooldown;
    }





    /*
    public void resetSpecialEnergy()
    {
        currentSpecialEnergy = 0f;
        specialReady = false;
    }

    /*
    private void updateSpecialEnergy()
    {
        if (specialReady) return;
        currentSpecialEnergy -= Time.deltaTime;
        specialReady = (currentSpecialCooldown <= 0f);
        currentSpecialCooldown = Mathf.Max(currentSpecialCooldown, 0f);
    }
    */

    /*
    public void addSpecialEnergy(float energy)
    {
        currentSpecialEnergy += energy;
        currentSpecialEnergy = Mathf.Min(currentSpecialEnergy, stats.ComputeValue("Special Energy Cost"));
        SaveManager.data.north.specialEnergy = currentSpecialEnergy;
        specialReady = (currentSpecialEnergy >= stats.ComputeValue("Special Energy Cost"));
    }

    public float getSpecialEnergy()
    {
        return currentSpecialEnergy;
    }
    */

    public bool getSpecialReady()
    {
        return specialReady;
    }

    public void setSpecialReady(bool ready)
    {
        specialReady = ready;
    }

    /*
    private void OnDamageSpecialEnergyGain(DamageContext context)
    {
        SpecialEnergyPool specialEnergyPool = context.victim.GetComponent<SpecialEnergyPool>();
        Health victimHealth = context.victim.GetComponent<Health>();
        if (specialEnergyPool == null || victimHealth == null || context.attacker != PlayerID.instance.gameObject) return;

        float maxHealth = victimHealth.GetStats().ComputeValue("Max Health");
        float percentHealthDamaged = context.damage / maxHealth;
        Debug.Log("Current Energy: " + currentSpecialEnergy + "  |  Earned Energy: " + (specialEnergyPool.energyPool * percentHealthDamaged));
        addSpecialEnergy(specialEnergyPool.energyPool * percentHealthDamaged);
    }
    */





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
