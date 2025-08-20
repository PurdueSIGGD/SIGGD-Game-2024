using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatheHitAndRun : MonoBehaviour
{
    [SerializeField] GameObject skullObject;
    Vector2 targetPosition;
    Rigidbody2D skullRb;
    [SerializeField] GameObject warningObject;
    LineRenderer renderer;
    [SerializeField] float driveBySpeed;
    [SerializeField] float driveByTime;
    [SerializeField] float warningDelaySec;
    [SerializeField] float warningBlinkSec;
    int direction;
    Func<GameObject, bool> removeFromList;

    public void Initialize(Transform player, Func<GameObject, bool> removeFromList)
    {
        skullRb = skullObject.GetComponent<Rigidbody2D>();
        renderer = warningObject.GetComponent<LineRenderer>();
        direction = Mathf.RoundToInt(UnityEngine.Random.value) == 0 ? 1 : -1;
        targetPosition = player.position;
        this.removeFromList = removeFromList;

        transform.position = targetPosition;
        transform.localScale = new Vector3(
            transform.localScale.x * direction,
            transform.localScale.y,
            transform.localScale.z
        );

        StartCoroutine(AttackSequenceCoroutine());
    }
    IEnumerator AttackSequenceCoroutine()
    {

        // warning logic

        warningObject.SetActive(true);
        StartCoroutine(WarningBlinkCoroutine());
        yield return new WaitForSeconds(warningDelaySec);
        StopCoroutine(WarningBlinkCoroutine());
        warningObject.SetActive(false);

        skullObject.SetActive(true);

        // drive by logic

        skullRb.velocity = Vector2.right * direction * driveBySpeed;
        yield return new WaitForSeconds(driveByTime);
        skullRb.velocity = Vector2.zero;
        skullObject.SetActive(false);

        // end logic

        removeFromList(gameObject);
        Destroy(gameObject);
    }
    IEnumerator WarningBlinkCoroutine()
    {
        while (true)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(warningBlinkSec);
        }
    }
}
