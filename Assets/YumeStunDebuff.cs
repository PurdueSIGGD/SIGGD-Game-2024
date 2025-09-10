using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class YumeStunDebuff : MonoBehaviour
{

    [SerializeField] private GameObject pulseVFX;
    [SerializeField] private Color pulseColor;

    [SerializeField] private DamageContext damageContext;
    [SerializeField] private float damage;
    [SerializeField] private float delay;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        //damageContext.damage = damage;
        //timer = delay;
        //GetComponentInParent<EnemyStateManager>().Stun(damageContext, 1.3f);
    }

    public void StartDebuff(float duration)
    {
        damageContext.damage = damage;
        timer = duration - delay;
        Debug.Log("WEAVES STUN TIME: " + duration);
        GetComponentInParent<EnemyStateManager>().Stun(damageContext, duration);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            GameObject pulse = Instantiate(pulseVFX, transform.position, Quaternion.identity);
            pulse.GetComponent<RingExplosionHandler>().playRingExplosion(3f, pulseColor);
            gameObject.GetComponentInParent<Health>()?.Damage(damageContext, PlayerID.instance.gameObject);
            Destroy(gameObject);
        }
    }
}
