//#define DEBUG_LOG
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
///  Yume instantly weaves an extra spool when a number of fatebound enemies are defeated.
///  Defeated Enemies Required: 5 | 4 | 3 | 2
/// </summary>
public class ScrapSaver : Skill
{

    public int numEnemiesDefeated = 0; // Counts up
    private SeamstressManager manager = null;

    [SerializeField]
    private List<int> values = new List<int>
    {
        0, 5, 4, 3, 2
    };
    public int pointIndex;
    public bool isSaving = false;



    private void Start()
    {
        manager = gameObject.GetComponent<SeamstressManager>();
        numEnemiesDefeated = SaveManager.data.yume.scrapSaverCount;
    }

    private void Update()
    {
        if (isSaving && manager.GetSpools() < manager.GetStats().ComputeValue("Max Spools"))
        {
            numEnemiesDefeated = 0;
            SaveManager.data.yume.scrapSaverCount = numEnemiesDefeated;
            isSaving = false;
            manager.AddSpools(1);
        }
    }

    /// <summary>
    /// Calculate the number of enemies needed until the next extra spool can be added.
    /// </summary>
    public int CalculateNumEnemiesNeeded()
    {
        //return -1 * GetPoints() + 6; // 5, 4, 3, 2
        return values[pointIndex];
    }

    public void HandleEnemyDefeated()
    {
        if (GetPoints() <= 0) { return; }

        if (isSaving) { return; }

        // Count up
        numEnemiesDefeated++;
        SaveManager.data.yume.scrapSaverCount = numEnemiesDefeated;

        AudioManager.Instance.SFXBranch.GetSFXTrack("Yume-Gained Spool").SetPitch(manager.GetSpools(), manager.GetStats().ComputeValue("Max Spools"));
        AudioManager.Instance.SFXBranch.PlaySFXTrack("Yume-Gained Spool");
        AudioManager.Instance.VABranch.PlayVATrack("Yume-Seamstress Recovered Wares");

        if (numEnemiesDefeated >= CalculateNumEnemiesNeeded())
        {
            GetComponent<YumeUIDriver>().skill1UIManager.pingAbility();

            numEnemiesDefeated = 0;
            SaveManager.data.yume.scrapSaverCount = numEnemiesDefeated;

            // Add spool
            if (manager.GetSpools() >= manager.GetStats().ComputeValue("Max Spools"))
            {
                isSaving = true;
                return;
            }
            manager.AddSpools(1);

#if DEBUG_LOG
            Debug.Log("Yume Scrap Saver: Added spool");
#endif


        }
        
#if DEBUG_LOG
        Debug.Log("Yume Scrap Saver: Enemies defeated/needed " + numEnemiesDefeated + "/" + CalculateNumEnemiesNeeded() + " points " + GetPoints());
#endif
    }

    public override void AddPointTrigger() { pointIndex = GetPoints(); }
    public override void ClearPointsTrigger() { pointIndex = GetPoints(); }
    public override void RemovePointTrigger() { pointIndex = GetPoints(); }

}
