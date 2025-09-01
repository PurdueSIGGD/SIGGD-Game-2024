using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleAndThreadDebuff : MonoBehaviour
{
    private int debuffIntensity;
    private float debuffDuration;
    private StatManager stats;

    public void Init(int intensity, float duration)
    {
        debuffIntensity = intensity;
        debuffDuration = duration;
        stats = GetComponent<StatManager>();
        ModifySpeed(-debuffIntensity);
    }

    void Update()
    {
        if (debuffDuration < 0)
        {
            ModifySpeed(debuffIntensity);
            Destroy(this);
        }
        else
        {
            debuffDuration -= Time.deltaTime;
        }
    }

    private void ModifySpeed(int intensity)
    {
        if (stats)
        {
            stats.ModifyStat("Speed", intensity);
            stats.ModifyStat("Idle Speed", intensity);
            if (gameObject.GetComponentInParent<EnemyStateManager>().isFlyer) stats.ModifyStat("FLIGHT_FORCE", intensity);
        }
    }
}
