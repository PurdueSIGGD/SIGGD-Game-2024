using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

     void Update()
    {
        Debug.Log("test + blue " + blueSpiritsCollected);
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
