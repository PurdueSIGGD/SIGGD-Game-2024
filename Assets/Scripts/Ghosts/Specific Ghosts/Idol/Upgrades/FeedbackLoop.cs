using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackLoop : Skill
{
    StatManager stats;
    List<int> values = new List<int>
    {
        0, 7, 14, 21, 28
    };
    int percentIncrease = 0;
    private int pointIndex;

    void Start()
    {
        stats = gameObject.GetComponent<StatManager>();
    }
    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
        UpdateSkill();
    }

    public override void ClearPointsTrigger()
    {
        pointIndex = GetPoints();
        UpdateSkill();
    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
        UpdateSkill();
    }
    /// <summary>
    /// Removes the old modifier and applies the new modifier.
    /// </summary>
    private void UpdateSkill()
    {
        stats.ModifyStat("Special Cooldown", percentIncrease);
        percentIncrease = values[pointIndex];
        stats.ModifyStat("Special Cooldown", -percentIncrease);
    }
}