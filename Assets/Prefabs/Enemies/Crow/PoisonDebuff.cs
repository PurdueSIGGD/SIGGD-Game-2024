using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached to a poision prefab that is attached to a target
/// </summary>
public class PoisonDebuff : MonoBehaviour
{

    Health playerHealth;
    [SerializeField] DamageContext damageContext;
    [SerializeField] int damage;
    [SerializeField] float time; // time before debuff ends
    [SerializeField] float interval; // seconds per tick
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ByeByeCoroutine(time));
        playerHealth = gameObject.GetComponentInParent<Health>();
        timer = interval;
        damageContext.damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        playerHealth = gameObject.GetComponentInParent<Health>();
        if (playerHealth != null)
        {
            playerHealth.Damage(damageContext, gameObject);
        }
        timer = interval;
    }
    IEnumerator ByeByeCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
