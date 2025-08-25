using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackLoop : Skill
{
    StatManager stats;
    IdolManager manager;
    [SerializeField] List<float> values = new List<float>
    {
        0, 7, 14, 21, 28
    };

    private int pointIndex;

    private float accumulatedCooldownReduction;

    void Start()
    {
        stats = gameObject.GetComponent<StatManager>();
        manager = gameObject.GetComponent<IdolManager>();
        accumulatedCooldownReduction = 0f;
    }

    private void Update()
    {
        if (accumulatedCooldownReduction > 0f && manager.getSpecialCooldown() > 0f)
        {
            manager.setSpecialCooldown(manager.getSpecialCooldown() - accumulatedCooldownReduction);
            accumulatedCooldownReduction = 0f;
        }
    }

    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void ClearPointsTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
    }

    public void reduceCooldown(bool guaranteeReduction)
    {
        if (pointIndex <= 0) return;
        float cooldownReduction = stats.ComputeValue("Special Cooldown") * (values[pointIndex] / 100f);
        if (manager.getSpecialCooldown() > 0)
        {
            manager.setSpecialCooldown(manager.getSpecialCooldown() - cooldownReduction);
        }
        else if (manager.clonesActive || guaranteeReduction)
        {
            accumulatedCooldownReduction += cooldownReduction;
        }
    }
}