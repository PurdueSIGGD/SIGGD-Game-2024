using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[DisallowMultipleComponent]
public class HeavyAttack : MonoBehaviour, IStatList
{
    [SerializeField]
    public StatManager.Stat[] statList;

    private Camera mainCamera;
    private StatManager stats;
    private PlayerStateMachine playerStateMachine;
    private OrionManager manager;

    [HideInInspector] private bool isCharging = false;
    [HideInInspector] public float chargingTime = 0f;
    [HideInInspector] private bool isPrimed = false;
    [HideInInspector] private float primedTime = 0f;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        stats = this.GetComponent<StatManager>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
        manager = GetComponent<OrionManager>();
    }

    private void Update()
    {
        if (isCharging && chargingTime > 0f) chargingTime -= Time.deltaTime;
        if (isCharging && chargingTime <= 0f) playerStateMachine.EnableTrigger("OPT");

        if (isPrimed && primedTime > 0f) primedTime -= Time.deltaTime;
        if (isPrimed && primedTime <= 0f) playerStateMachine.EnableTrigger("OPT");

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDiff = transform.position - mousePos;
        if (isCharging || isPrimed) {
            if (mouseDiff.x < 0) // update player facing direction
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (mouseDiff.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    public void StartHeavyChargeUp()
    {
        GetComponent<Move>().PlayerStop();
        chargingTime = manager.GetStats().ComputeValue("Heavy Charge Up Time");
        isCharging = true;
        AudioManager.Instance.SFXBranch.PlaySFXTrack("HeavyAttackWindUp");
    }

    public void StopHeavyChargeUp()
    {
        GetComponent<Move>().PlayerGo();
        isCharging = false;
        chargingTime = 0f;
    }

    public void StartHeavyPrimed()
    {
        GetComponent<Move>().PlayerStop();
        primedTime = manager.GetStats().ComputeValue("Heavy Primed Autofire Time");
        isPrimed = true;
        AudioManager.Instance.SFXBranch.PlaySFXTrack("HeavyAttackPrimed");
    }

    public void StopHeavyPrimed()
    {
        GetComponent<Move>().PlayerGo();
        isPrimed = false;
        primedTime = 0f;
    }

    public void StartHeavyAttack()
    {
        GetComponent<Move>().PlayerStop();
        playerStateMachine.ConsumeHeavyAttackInput();
    }

    public void ExecuteHeavyAttack()
    {
        GetComponent<PlayerParticles>().PlayHeavyAttackVFX();
        playerStateMachine.SetLightAttack2Ready(false);
        CameraShake.instance.Shake(0.2f, 10f, 0, 10, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
        RaycastHit2D[] hits = Physics2D.BoxCastAll(manager.heavyIndicator.transform.position, manager.heavyIndicator.transform.localScale, 0, new Vector2(0, 0));
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Heavy Attack Hit: " + hit.transform.gameObject.name);
                hit.transform.gameObject.GetComponent<Health>().Damage(manager.heavyDamage, gameObject);
            }
        }
        AudioManager.Instance.VABranch.PlayVATrack(PartyManager.instance.selectedGhost + " Heavy Attack");
        AudioManager.Instance.SFXBranch.PlaySFXTrack("HeavyAttack");
    }

    public void StopHeavyAttack()
    {
        GetComponent<Move>().PlayerGo();
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
