using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HeavyAttack : MonoBehaviour
{
    [SerializeField] GameObject indicator;
    //[SerializeField] int dmg;
    [SerializeField] DamageContext heavyDamage;
    [SerializeField] float offsetX;
    private Camera mainCamera;
    private float timer;

    private void Start()
    {
        indicator.SetActive(false);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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
        indicator.SetActive(true);
        timer = 0.5f;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(indicator.transform.position, indicator.transform.localScale, 0, new Vector2(0, 0));
        foreach(RaycastHit2D hit in hits)
        {
            if(hit.transform.gameObject.tag == "Enemy")
            {
                Debug.Log("Heavy Attack Hit: " + hit.transform.gameObject.name);
                /*
                IDamageable enemyhealth = hit.transform.gameObject.GetComponent<IDamageable>();
                if (enemyhealth != null)
                {
                    //ehealth.TakeDamage(dmg);
                    enemyhealth.Damage(heavyDamage, gameObject);
                }
                */
                hit.transform.gameObject.GetComponent<Health>().Damage(heavyDamage, gameObject);
            }
        }
    }
}
