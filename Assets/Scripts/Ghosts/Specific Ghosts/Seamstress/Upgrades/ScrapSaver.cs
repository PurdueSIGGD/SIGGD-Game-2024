//#define DEBUG_LOG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Yume instantly weaves an extra spool when a number of fatebound enemies are defeated.
///  Defeated Enemies Required: 5 | 4 | 3 | 2
/// </summary>
public class ScrapSaver : Skill
{

    private int numEnemiesDefeated = 0; // Counts up
    private SeamstressManager manager = null;

    private void Start()
    {
        manager = gameObject.GetComponent<SeamstressManager>();
    }

    /// <summary>
    /// Calculate the number of enemies needed until the next extra spool can be added.
    /// </summary>
    private int CalculateNumEnemiesNeeded()
    {
        return -1 * GetPoints() + 6; // 5, 4, 3, 2
    }

    public void HandleEnemyDefeated()
    {
        if (GetPoints() <= 0) { return; }

        // Count up
        numEnemiesDefeated++;

        if (numEnemiesDefeated >= CalculateNumEnemiesNeeded())
        {
            numEnemiesDefeated = 0;

            // Add spool
            manager.AddSpools(1);

#if DEBUG_LOG
            Debug.Log("Yume Scrap Saver: Added spool");
#endif


        }

#if DEBUG_LOG
        Debug.Log("Yume Scrap Saver: Enemies defeated/needed " + numEnemiesDefeated + "/" + CalculateNumEnemiesNeeded() + " points " + GetPoints());
#endif
    }

    public override void AddPointTrigger() {}
    public override void ClearPointsTrigger() {}
    public override void RemovePointTrigger() {}

}
