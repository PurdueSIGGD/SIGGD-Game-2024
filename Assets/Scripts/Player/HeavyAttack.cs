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

    //[SerializeField] GameObject indicator;
    //[SerializeField] int dmg;
    //[SerializeField] DamageContext heavyDamage;
    //[SerializeField] float offsetX;
    private Camera mainCamera;
    private float timer;
    private StatManager stats;
    private PlayerStateMachine playerStateMachine;
    private OrionManager manager;

    //private bool doingHeavyChargeUp;
    //private bool doingHeavyPrimed;

    [HideInInspector] private bool isCharging = false;
    [HideInInspector] public float chargingTime = 0f;
    [HideInInspector] private bool isPrimed = false;
    [HideInInspector] private float primedTime = 0f;

    private void Start()
    {
        //indicator.SetActive(false);
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

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            manager.heavyIndicator.SetActive(false);
        }
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < transform.position.x)
        {
            manager.heavyIndicator.transform.localPosition = new Vector3(-manager.offsetX, manager.heavyIndicator.transform.localPosition.y, 0);
        }
        else
        {
            manager.heavyIndicator.transform.localPosition = new Vector3(manager.offsetX, manager.heavyIndicator.transform.localPosition.y, 0);
        }

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
        chargingTime = manager.GetStats().ComputeValue("Heavy Charge Up Time");
        isCharging = true;
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
    }

    public void StopHeavyPrimed()
    {
        isPrimed = false;
        primedTime = 0f;
    }

    public void StartHeavyAttack()
    {
        //indicator.SetActive(true);
        timer = 0.5f;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(manager.heavyIndicator.transform.position, manager.heavyIndicator.transform.localScale, 0, new Vector2(0, 0));
        foreach(RaycastHit2D hit in hits)
        {
            if(hit.transform.gameObject.tag == "Enemy")
            {
                Debug.Log("Heavy Attack Hit: " + hit.transform.gameObject.name);

                foreach (IDamageable damageable in hit.transform.gameObject.GetComponents<IDamageable>()) {
                    //heavyDamage.damage = stats.ComputeValue("Heavy Damage");
                    damageable.Damage(manager.heavyDamage, gameObject);
                }

                /*
                IDamageable enemyhealth = hit.transform.gameObject.GetComponent<IDamageable>();
                if (enemyhealth != null)
                {
                    //ehealth.TakeDamage(dmg);
                    enemyhealth.Damage(heavyDamage, gameObject);
                }
                */
                //heavyDamage.damage = stats.ComputeValue("Heavy Damage");
                //hit.transform.gameObject.GetComponent<Health>().Damage(heavyDamage, gameObject);
            }
        }
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
