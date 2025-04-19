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
    void Start()
    {
        stats = gameObject.GetComponent<StatManager>();

        // testing code

        // int points = 4;
        // for (int i = 0; i < points; i++)
        // {
        //     AddPoint();
        // }
    }
    public override void AddPointTrigger()
    {
        UpdateSkill();
    }

    public override void ClearPointsTrigger()
    {
        UpdateSkill();
    }

    public override void RemovePointTrigger()
    {
        UpdateSkill();
    }
    /// <summary>
    /// Removes the old modifier and applies the new modifier.
    /// </summary>
    private void UpdateSkill()
    {
        stats.ModifyStat("Special Cooldown", percentIncrease);
        percentIncrease = values[skillPts];
        stats.ModifyStat("Special Cooldown", -percentIncrease);
    }
}