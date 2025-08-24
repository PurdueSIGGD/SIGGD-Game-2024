using System;
using System.Collections;
using UnityEngine;

public class ScatheSwipe : MonoBehaviour
{
    [SerializeField] GameObject warningObject;
    [SerializeField] float warningDelaySec;
    [SerializeField] float warningBlinkSec;
    Animator anim;
    Collider2D col;
    Func<GameObject, bool> removeFromList;
    [SerializeField] DamageContext damageContext;
    [SerializeField] float damage;
    [SerializeField] GameObject attacker;

    public void Initialize(Func<GameObject, bool> removeFromList)
    {
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        this.removeFromList = removeFromList;
        StartCoroutine(AttackSequenceCoroutine());

        damageContext.damage = damage;
    }

    public void TurnDamageOn()
    {
        col.enabled = true;
    }
    public void TurnDamageOff()
    {
        col.enabled = false;
    }
    public void DestroyObject()
    {
        removeFromList(gameObject);
        Destroy(gameObject);
    }
    IEnumerator AttackSequenceCoroutine()
    {

        // warning logic

        warningObject.SetActive(true);
        StartCoroutine(WarningBlinkCoroutine());
        yield return new WaitForSeconds(warningDelaySec);
        StopCoroutine(WarningBlinkCoroutine());
        Destroy(warningObject);

        // swipe logic

        anim.SetTrigger("start");
    }

    IEnumerator WarningBlinkCoroutine()
    {
        while (warningObject != null)
        {
            warningObject.SetActive(!warningObject.activeSelf);
            yield return new WaitForSeconds(warningBlinkSec);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BossController>() != null)
            return;

        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Enemy"))
        {
            Health hp = collision.gameObject.GetComponent<Health>();
            hp.Damage(damageContext, attacker);
        }
    }
}
