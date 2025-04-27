using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStopper : Skill
{
    [SerializeField] float healthRestored = 30f;
    [SerializeField] float timeStopDuration = 5f;
    bool expired; // tracks if ability is already used before

    TimeFreezeManager freezeManager;

    void Start()
    {
        freezeManager = GetComponent<TimeFreezeManager>();
        expired = false;
        AddPoint();
    }

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageFilter.Add(ShowStop);
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageFilter.Remove(ShowStop);
    }

    void Update()
    {
        // TODO remove
        // cheeky little shortcut
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(Freeze(timeStopDuration));
        }
    }

    void ShowStop(ref DamageContext context)
    {
        if (skillPts == 0 || expired) { return; }

        if (context.victim.CompareTag("Player"))
        {
            Health health = context.victim.GetComponent<Health>();
            if (context.damage >= health.currentHealth)
            {
                context.damage = 0f;
                health.currentHealth = healthRestored;
                expired = true;
                TimeFreeze();
            }
        }
    }

    void TimeFreeze()
    {
        StartCoroutine(Freeze(timeStopDuration));
    }


    IEnumerator Freeze(float duration)
    {
        freezeManager.FreezeTime(duration);
        yield return new WaitForSeconds(duration);
        freezeManager.UnFreezeTime();
    }

    public override void AddPointTrigger()
    {
    }

    public override void ClearPointsTrigger()
    {
    }

    public override void RemovePointTrigger()
    {
    }
}
