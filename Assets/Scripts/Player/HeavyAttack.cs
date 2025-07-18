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
        if (isCharging || isPrimed)
        {
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
        playerStateMachine.SetLightAttack2Ready(true);
        chargingTime = manager.GetStats().ComputeValue("Heavy Charge Up Time");
        isCharging = true;
        AudioManager.Instance.SFXBranch.PlaySFXTrack("HeavyAttackWindUp");
        manager.heavyDamage.damage = manager.GetStats().ComputeValue("Heavy Damage");
        manager.heavyDamage.damageStrength = DamageStrength.MODERATE;
    }

    public void StopHeavyChargeUp()
    {
        isCharging = false;
        chargingTime = 0f;
    }

    public void StartHeavyPrimed()
    {
        primedTime = manager.GetStats().ComputeValue("Heavy Primed Autofire Time");
        isPrimed = true;
        AudioManager.Instance.SFXBranch.PlaySFXTrack("HeavyAttackPrimed");
        manager.heavyDamage.damage = manager.GetStats().ComputeValue("Super Heavy Damage");
        manager.heavyDamage.damageStrength = DamageStrength.HEAVY;
    }

    public void StopHeavyPrimed()
    {
        isPrimed = false;
        primedTime = 0f;
    }

    public void StartHeavyAttack()
    {
        //playerStateMachine.ConsumeHeavyAttackInput();
        AudioManager.Instance.VABranch.PlayVATrack(PartyManager.instance.selectedGhost + " Heavy Attack");
    }

    public void ExecuteHeavyAttack()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("HeavyAttack");
        Vector3 vfxPosition = gameObject.transform.position + new Vector3(2f * Mathf.Sign(gameObject.transform.rotation.y), 0.45f, 0f);
        VFXManager.Instance.PlayVFX(VFX.PLAYER_HEAVY_ATTACK, vfxPosition, gameObject.transform.rotation);
        playerStateMachine.SetLightAttack2Ready(false);
        CameraShake.instance.Shake(0.2f, 10f, 0, 10, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
        RaycastHit2D[] hits = Physics2D.BoxCastAll(manager.heavyIndicator.transform.position, manager.heavyIndicator.transform.localScale, 0, new Vector2(0, 0));
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Heavy Attack Hit: " + hit.transform.gameObject.name);
                manager.heavyDamage.raycastHitPosition = new Vector2(transform.position.x, transform.position.y);
                hit.transform.gameObject.GetComponent<Health>().Damage(manager.heavyDamage, gameObject);
                if (hit.transform.gameObject.GetComponent<EnemyStateManager>() != null)
                {
                    hit.transform.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(Vector3.up, 6f, 0.3f);
                    hit.transform.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(hit.transform.position - gameObject.transform.position, 5f, 0.3f);
                }
            }
        }
    }

    public void StopHeavyAttack()
    {

    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
