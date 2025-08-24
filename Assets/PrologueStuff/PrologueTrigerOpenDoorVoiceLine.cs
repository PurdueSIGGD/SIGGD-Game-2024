using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueTrigerOpenDoorVoiceLine : MonoBehaviour
{
    [SerializeField] float timeToWaitBeforeSelfDestruct;
    [SerializeField] FadeInUI subtitle;
    [SerializeField] AudioSource source;

    private IEnumerator enumerator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enumerator == null && collision.gameObject == PlayerID.instance.gameObject)
        {
            source.Play();
            enumerator = WaitToDestroy();
            StartCoroutine(enumerator);
        }
    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(timeToWaitBeforeSelfDestruct);
        subtitle.DisenablePrompt();
        Destroy(this);
    }
}
