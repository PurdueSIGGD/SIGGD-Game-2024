using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritTrackerCanvasUI : MonoBehaviour
{
    [SerializeField] List<SpiritCounterUI> spiritCounters;

    // Start is called before the first frame update
    void Start()
    {
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
