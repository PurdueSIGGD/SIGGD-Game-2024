#define DEBUG_LOG
using UnityEngine;

/// <summary>
/// Heavy attacks that are empowered by Yume's spools stun enemies for a longer duration.
/// Bonus Stun Time: +0.3s | +0.6s | +0.9s | +1.2s
/// </summary>
public class EntanglingWeaves : Skill
{
    private const string STUN_DURATION_STAT = "Spool Heavy Attack Stun";
    StatManager stats;

    private void Start()
    {
        stats = gameObject.GetComponent<StatManager>();
    }

    private void BuffStunDuration()
    {

        stats.SetStat(STUN_DURATION_STAT, 100);

        if (GetPoints() <= 0)
        {
            return;
        }

        // Calculate stat modifier and set stat

        float extraStun = 0.3f * GetPoints(); // calculate bonus stun time
        int modifier = (int) (100 * (stats.ComputeValue(STUN_DURATION_STAT) + extraStun) / stats.ComputeValue(STUN_DURATION_STAT));

        stats.SetStat(STUN_DURATION_STAT, modifier);

#if DEBUG_LOG
        Debug.Log("Entangling weaves: stun duration now " + stats.ComputeValue(STUN_DURATION_STAT) + " points " + GetPoints());
#endif

    }


    public override void AddPointTrigger()
    {
        BuffStunDuration();
    }

    public override void ClearPointsTrigger()
    {
        BuffStunDuration();
    }

    public override void RemovePointTrigger()
    {
        BuffStunDuration();
    }
}
