using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostManager : MonoBehaviour, ISelectable, IStatList
{
    [SerializeField] public StatManager.Stat[] statList;
    [SerializeField] public RuntimeAnimatorController defaultController;
    [SerializeField] public RuntimeAnimatorController ghostController;
    
    private Animator animator;
    private StatManager stats;

    private float currentBasicCooldown = 0f;
    private bool basicReady = true;

    private float currentSpecialCooldown = 0f;
    private bool specialReady = true;



    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = PlayerID.instance.GetComponent<Animator>();
        stats = GetComponent<StatManager>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        updateSpecialCooldown();
    }

    public virtual void Select(GameObject player)
    {
        Destroy(PlayerID.instance.GetComponent<Dash>());
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


    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
