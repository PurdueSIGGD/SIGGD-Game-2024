using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Spirit;

public class SpiritTracker : MonoBehaviour
{
    public int blueSpiritsCollected = 0;
    public int redSpiritsCollected = 0;
    public int yellowSpiritsCollected = 0;

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
        }
    }

    public bool SpendSpirits(Spirit.SpiritType spiritType, int price)
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

        }

        return true;
    }

    /// <summary>
    /// Call when user chooses to transfer collected spirits to the Hub
    /// </summary>
    public void SaveSpiritCounts()
    {
        SaveManager.data.spiritCounts[0] += redSpiritsCollected;
        SaveManager.data.spiritCounts[1] += blueSpiritsCollected;
        SaveManager.data.spiritCounts[2] += yellowSpiritsCollected;

        redSpiritsCollected = 0;
        blueSpiritsCollected = 0;
        yellowSpiritsCollected = 0;
    }
}
