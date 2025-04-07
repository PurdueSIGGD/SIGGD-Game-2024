using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
