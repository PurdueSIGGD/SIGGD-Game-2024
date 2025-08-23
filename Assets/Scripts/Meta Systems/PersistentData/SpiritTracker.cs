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

        // UI
        SpiritTrackerCanvasUI.Instance.UpdateCounters();
        
    }


    /// <summary>
    /// Returns true if player has enough spirits to buy something.
    /// </summary>
    /// <param name="secured">Whether the spirit is secured/hub spirit or a temporary spirit.</param>
    /// <param name="spiritType"></param>
    /// <param name="price"></param>
    /// <returns></returns>
    public bool HasEnoughSpirits(bool secured, SpiritType spiritType, int price)
    {
        if (secured)
        {
            return SaveManager.data.spiritCounts[(int)spiritType] >= price;
        }
        else
        {
            switch (spiritType)
            {
                case SpiritType.Blue:
                    return price <= blueSpiritsCollected;
                    break;
                case SpiritType.Red:
                    return price <= redSpiritsCollected;
                    break;
                case SpiritType.Yellow:
                    return price <= yellowSpiritsCollected;
                case SpiritType.Pink:
                    return price <= pinkSpiritsCollected;
                default: return false;
            }
        }
    }

    public bool SpendSecuredSpirits(Spirit.SpiritType spiritType, int price)
    {
        if (!HasEnoughSpirits(true, spiritType, price))
        {
            // Not enough spirits
            return false;
        }
        else
        {
            // Spend spirits
            SaveManager.data.spiritCounts[(int)spiritType] -= price;

            // UI
            SpiritTrackerCanvasUI.Instance.UpdateCounters();

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
        SpiritTrackerCanvasUI.Instance.UpdateCounters();

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

        // UI
        SpiritTrackerCanvasUI.Instance.UpdateCounters();
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

        ClearSpirits();

        // UI
        SpiritTrackerCanvasUI.Instance.UpdateCounters();
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
