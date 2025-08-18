using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Spirit;

public class SpiritTracker : MonoBehaviour
{
    public int blueSpiritsCollected = 0;
    public int redSpiritsCollected = 0;
    public int yellowSpiritsCollected = 0;
    public int pinkSpiritsCollected = 0;

    [HideInInspector]
    public SpiritTrackerCanvasUI trackerUI; // reference to tracker UI

    private void OnEnable()
    {
        Spirit.SpiritCollected += CollectSpirit;
    }

    private void OnDisable()
    {
        Spirit.SpiritCollected -= CollectSpirit;
    }

    private void CollectSpirit(Spirit.SpiritType spiritType)
    {
        switch (spiritType)
        {
            case Spirit.SpiritType.Blue:
                blueSpiritsCollected++;
                break;
            case Spirit.SpiritType.Red:
                redSpiritsCollected++;
                break;
            case Spirit.SpiritType.Yellow:
                yellowSpiritsCollected++;
                break;
            case Spirit.SpiritType.Pink:
                pinkSpiritsCollected++;
                break;
        }

        if (trackerUI != null)
        {
            trackerUI.UpdateCounters();
        }
    }

    public bool SpendSecuredSpirits(Spirit.SpiritType spiritType, int price)
    {
        if (SaveManager.data.spiritCounts[(int) spiritType] < price)
        {
            // Not enough spirits
            return false;
        }
        else
        {
            // Spend spirits
            SaveManager.data.spiritCounts[(int)spiritType] -= price;

            // UI
            if (trackerUI)
            {
                trackerUI.UpdateCounters();
            }

            return true;
        }
    }

    public bool SpendRunSpirits(Spirit.SpiritType spiritType, int price)
    {
        switch (spiritType)
        {
            case Spirit.SpiritType.Red:
                if (redSpiritsCollected >= price)
                {
                    redSpiritsCollected -= price;
                }
                else
                {
                    return false;
                }
                break;
            case Spirit.SpiritType.Yellow:
                if (yellowSpiritsCollected >= price)
                {
                    yellowSpiritsCollected -= price;
                }
                else
                {
                    return false;
                }
                break;
            case Spirit.SpiritType.Blue:
                if (blueSpiritsCollected >= price)
                {
                    blueSpiritsCollected -= price;
                }
                else
                {
                    return false;
                }
                break;
            case Spirit.SpiritType.Pink:
                if (pinkSpiritsCollected >= price)
                {
                    pinkSpiritsCollected -= price;
                }
                else
                {
                    return false;
                }
                break;
        }

        // UI
        if (trackerUI)
        {
            trackerUI.UpdateCounters();
        }

        return true;
    }

    /// <summary>
    /// Add a certain number of one color of spirits (secured)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="numSpirits"></param>
    public void AddSecuredSpirits(SpiritType type, int numSpirits)
    {
        SaveManager.data.spiritCounts[(int)type] += numSpirits;
        if (trackerUI)
        {
            trackerUI.UpdateCounters();
        }
    }

    /// <summary>
    /// Call when user chooses to transfer collected spirits to the Hub
    /// </summary>
    public void SaveSpiritCounts()
    {
        SaveManager.data.spiritCounts[0] += blueSpiritsCollected;
        SaveManager.data.spiritCounts[1] += redSpiritsCollected;
        SaveManager.data.spiritCounts[2] += yellowSpiritsCollected;
        SaveManager.data.spiritCounts[3] += pinkSpiritsCollected;

        // UI
        if (trackerUI)
        {
            trackerUI.UpdateCounters();
        }

        ClearSpirits();
    }

    /// <summary>
    /// Set run spirit counts to 0
    /// </summary>
    public void ClearSpirits()
    {
        redSpiritsCollected = 0;
        blueSpiritsCollected = 0;
        yellowSpiritsCollected = 0;
        pinkSpiritsCollected = 0;
    }
}
