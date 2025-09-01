using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStopper : Sacrifice
{
    [SerializeField] float healthRestored = 30f;
    [SerializeField] float timeStopDuration = 5f;
    bool expired; // tracks if ability is already used before

    TimeFreezeManager freezeManager;

    void Start()
    {
        freezeManager = GetComponent<TimeFreezeManager>();
        expired = false;
    }

    //private void OnEnable()
    //{
    //    GameplayEventHolder.OnDamageFilter.Add(ShowStop);
    //}

    //private void OnDisable()
    //{
    //    GameplayEventHolder.OnDamageFilter.Remove(ShowStop);
    //}

    void ShowStop(ref DamageContext context)
    {
        if (expired) { return; }

        if (context.victim.CompareTag("Player"))
        {
            PartyManager.instance.ChangePosessingGhost(-1);

            Health health = context.victim.GetComponent<Health>();
            if (context.damage >= health.currentHealth)
            {
                // play audio
                AudioManager.Instance.VABranch.PlayVATrack("Eva-Idol Show Stopper Before");

                //context.damage = 0f;
                //health.currentHealth = healthRestored;
                //expired = true;
                //TimeFreeze();
            }
        }
    }

    public override void DoSac()
    {
        base.DoSac();
        AudioManager.Instance.VABranch.PlayVATrack("Eva-Idol Show Stopper Before");
        TimeFreeze();
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

        AudioManager.Instance.VABranch.PlayVATrack("Eva-Idol Show Stopper After");
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
