using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStopper : MonoBehaviour
{
    TimeFreezeManager freezeManager;
    Rigidbody2D rb;

    void Start()
    {
        freezeManager = GetComponent<TimeFreezeManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(Freeze(5));
        }
    }

    IEnumerator Freeze(float duration)
    {
        freezeManager.FreezeTime(duration);
        yield return new WaitForSeconds(duration);
        freezeManager.UnFreezeTime();
    } 
}
