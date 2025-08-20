using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public void Initialize(Transform player, bool spawnOneSkull)
    {
        if (spawnOneSkull) Destroy(gameObject);

        skullRb = skullObject.GetComponent<Rigidbody2D>();
        renderer = warningObject.GetComponent<LineRenderer>();
        direction = Mathf.RoundToInt(Random.value) == 0 ? 1 : -1;
        targetPosition = player.position;

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
