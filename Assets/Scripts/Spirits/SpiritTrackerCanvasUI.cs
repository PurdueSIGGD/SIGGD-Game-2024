using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritTrackerCanvasUI : MonoBehaviour
{
    [SerializeField] List<SpiritCounterUI> spiritCounters;
    [SerializeField] bool fromSaveManager = true;
    void OnEnable()
    {
        SpiritTracker spiritTracker = PersistentData.Instance.GetComponent<SpiritTracker>();

        if (!spiritTracker.trackerUI)
        {
            spiritTracker.trackerUI = this;
        }

        foreach (SpiritCounterUI counter in spiritCounters)
        {
            counter.fromSaveManager = fromSaveManager;
        }

        UpdateCounters();
    }

    public void UpdateCounters()
    {
        foreach (SpiritCounterUI ui in spiritCounters)
        {
            ui.UpdateText();
        }
    }
}
