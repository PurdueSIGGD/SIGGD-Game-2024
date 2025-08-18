using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritTrackerCanvasUI : MonoBehaviour
{
    [SerializeField] List<SpiritCounterUI> spiritCounters;

    // Start is called before the first frame update
    void OnEnable()
    {
        SpiritTracker spiritTracker = PersistentData.Instance.GetComponent<SpiritTracker>();

        if (!spiritTracker.trackerUI)
        {
            spiritTracker.trackerUI = this;
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
