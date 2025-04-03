using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blight : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] int speedDebuff;
    [SerializeField] float dot;
    [SerializeField] DamageContext damageContext;
    [SerializeField] float deadlySeverity;

    private bool deadly = false;
    public GameObject effectParent;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInParent<StatManager>().ModifyStat("Speed", -speedDebuff);
        Debug.Log(gameObject.name);
        IEnumerator coroutine = DestroyInDuration(duration);
        StartCoroutine(coroutine);
        IEnumerator dotcoroutine = DOT(0.5f);
        StartCoroutine(dotcoroutine);

        GameplayEventHolder.OnDamageDealt += Damaged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Damaged(DamageContext damageContext)
    {
        if (effectParent != null && damageContext.victim == effectParent && !damageContext.damageTypes.Contains(DamageType.STATUS)) {
            WrapperCoroutine(0.75f);

        }
    }

    private void WrapperCoroutine(float waitime)
    {
        StartCoroutine(FinishDeadly(waitime));
    }

    private IEnumerator DestroyInDuration(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if(!deadly)
            gameObject.GetComponentInParent<StatManager>().ModifyStat("Speed", speedDebuff);
        else
            gameObject.GetComponentInParent<StatManager>().ModifyStat("Speed", speedDebuff + -(int)(speedDebuff * deadlySeverity));
        Destroy(gameObject);
    }

    private IEnumerator DOT(float waitTime)
    {
        while (true)
        {
            damageContext.damage = dot;
            Dmg(damageContext);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void Dmg(DamageContext damageContext)
    {
        gameObject.GetComponentInParent<Health>().Damage(damageContext, gameObject);
    }

    private IEnumerator FinishDeadly(float waitTime)
    {
        dot *= deadlySeverity;
        gameObject.GetComponentInParent<StatManager>().ModifyStat("Speed", -(int)(speedDebuff * deadlySeverity));
        deadly = true;
        yield return new WaitForSeconds(waitTime);
        deadly = false;
        dot /= deadlySeverity;
        gameObject.GetComponentInParent<StatManager>().ModifyStat("Speed", (int)(speedDebuff * deadlySeverity));
    }
}
