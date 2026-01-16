using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrisLaser : MonoBehaviour
{

    GameObject sourceObject;
    Collider2D col;

    [SerializeField] GameObject visualObject;
    [SerializeField] GameObject laserVisual;
    [SerializeField] GameObject laserWarning;
    [SerializeField] float damage;
    [SerializeField] DamageContext laserDamageContext;
    [SerializeField] float totalWarningTimeSec;
    [SerializeField] float laserActiveTimeSec;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        laserDamageContext.damage = damage;
    }

    public void Initialize(GameObject sourceObject)
    {
        this.sourceObject = sourceObject;
        visualObject.SetActive(false);
    }

    public void FireSequence(GameObject target)
    {
        StopCoroutine(FireSequenceCoroutine());

        // starting configuration
        col.enabled = false;
        visualObject.SetActive(true);
        laserWarning.SetActive(false);
        laserVisual.SetActive(false);

        // calculate and apply rotation to laser
        Vector2 targetPos = target.GetComponent<Transform>().position;
        Vector2 sourcePos = this.transform.position;
        Vector2 direction = (targetPos - sourcePos).normalized;
        float eulerAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, eulerAngle));

        StartCoroutine(FireSequenceCoroutine());
    }
    IEnumerator FireSequenceCoroutine()
    {
        // warning blinks
        int numBlinks = 8;
        int n = numBlinks * 2;
        float timePerIter = totalWarningTimeSec / n;
        bool warningOn = true;
        for (int i = 0; i < n; i++)
        {
            laserWarning.SetActive(warningOn);
            warningOn = !warningOn;
            yield return new WaitForSeconds(timePerIter);
        }

        // activate laser
        laserWarning.SetActive(false);
        laserVisual.SetActive(true);
        col.enabled = true;
        yield return new WaitForSeconds(laserActiveTimeSec);

        // end fire sequence
        EndFireSequence();
    }

    void EndFireSequence()
    {
        col.enabled = false;
        laserVisual.SetActive(false);
        visualObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        print("ILASER COLLIDING");
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHP = collision.gameObject.GetComponent<Health>();
            if (playerHP != null)
            {
                playerHP.Damage(laserDamageContext, sourceObject);
            }
        }
    }

    public void Stop()
    {
        EndFireSequence();
    }
}
