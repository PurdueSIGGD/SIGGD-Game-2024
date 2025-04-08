using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[DisallowMultipleComponent]
public class HeavyAttack : MonoBehaviour, IStatList
{
    [SerializeField]
    public StatManager.Stat[] statList;

    [SerializeField] GameObject indicator;
    //[SerializeField] int dmg;
    [SerializeField] DamageContext heavyDamage;
    [SerializeField] float offsetX;
    [SerializeField] LayerMask heavyLayerMask;
    private Camera mainCamera;
    private float timer;
    private StatManager stats;

    private void Start()
    {
        indicator.SetActive(false);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        stats = this.GetComponent<StatManager>();
    }

    private void Update()
    {
        if(timer > 0)
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
    }

    public void StartHeavyAttack()
    {
        //indicator.SetActive(true);
        timer = 0.5f;

        Vector3 hitPos;
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < transform.position.x)
        {
            hitPos = new Vector3(-offsetX, 0, 0);
        }
        else
        {
            hitPos = new Vector3(offsetX, 0, 0);
        }

        Debug.DrawLine(hitPos + transform.position, new Vector3(hitPos.x, 5, 0) + transform.position, Color.black, 5f);
        Collider2D[] coll = Physics2D.OverlapBoxAll(hitPos + transform.position, new Vector2(5, 5), 0);
        // RaycastHit2D[] hits = Physics2D.BoxCastAll(hitPos + transform.position, new Vector2(5, 5), 0, new Vector2(0, 0), heavyLayerMask);
        foreach(Collider2D hit in coll)
        {
            if(hit.transform.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Heavy Attack Hit: " + hit.transform.gameObject.name);
                // hit.transform.gameObject.GetComponent<Health>();
                heavyDamage.damage = stats.ComputeValue("Heavy Damage");
                foreach (IDamageable damageable in hit.transform.gameObject.GetComponents<IDamageable>())
                { 
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
                hit.transform.gameObject.GetComponent<Health>().Damage(heavyDamage, gameObject);
            }
        }
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
