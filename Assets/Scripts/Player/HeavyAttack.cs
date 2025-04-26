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

    [SerializeField] GameObject indicator;
    //[SerializeField] int dmg;
    [SerializeField] DamageContext heavyDamage;
    [SerializeField] float offsetX;
    private Camera mainCamera;
    private float timer;
    private StatManager stats;
    private bool doingHeavyChargeUp;
    private bool doingHeavyPrimed;

    private void Start()
    {
        indicator.SetActive(false);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        stats = this.GetComponent<StatManager>();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            indicator.SetActive(false);
        }
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < transform.position.x)
        {
            indicator.transform.localPosition = new Vector3(-offsetX, indicator.transform.localPosition.y, 0);
        }
        else
        {
            indicator.transform.localPosition = new Vector3(offsetX, indicator.transform.localPosition.y, 0);
        }

        Vector3 mouseDiff = transform.position - mousePos;

        if (doingHeavyChargeUp || doingHeavyPrimed) {
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
        doingHeavyChargeUp = true;
    }

    public void StopHeavyChargeUp()
    {
        doingHeavyChargeUp = false;
    }

    public void StartHeavyPrimed()
    {
        doingHeavyPrimed= true;
    }

    public void StopHeavyPrimed()
    {
        doingHeavyPrimed = false;
    }

    public void StartHeavyAttack()
    {
        //indicator.SetActive(true);
        timer = 0.5f;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(indicator.transform.position, indicator.transform.localScale, 0, new Vector2(0, 0));
        foreach(RaycastHit2D hit in hits)
        {
            if(hit.transform.gameObject.tag == "Enemy")
            {
                Debug.Log("Heavy Attack Hit: " + hit.transform.gameObject.name);

                foreach(IDamageable damageable in hit.transform.gameObject.GetComponents<IDamageable>()){
                    damageable.Damage(heavyDamage, gameObject);
                }

                /*
                IDamageable enemyhealth = hit.transform.gameObject.GetComponent<IDamageable>();
                if (enemyhealth != null)
                {
                    //ehealth.TakeDamage(dmg);
                    enemyhealth.Damage(heavyDamage, gameObject);
                }
                */
                heavyDamage.damage = stats.ComputeValue("Heavy Damage");
                //hit.transform.gameObject.GetComponent<Health>().Damage(heavyDamage, gameObject);
            }
        }
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
