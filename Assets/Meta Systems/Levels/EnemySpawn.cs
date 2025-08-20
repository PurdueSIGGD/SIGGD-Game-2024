using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawn
{
    [SerializeField] GameObject enemy;
    [SerializeField] ChanceRange[] chanceRanges;

    public float GetChance(float progress)
    {
        ChanceRange chanceRange = new ChanceRange();
        chanceRange.start = -1;
        float totalSpace = -1;
        for (int i = 0; i < chanceRanges.Length - 1; i++)
        {
            if (chanceRanges[i+1].start >= progress)
            {
                chanceRange = chanceRanges[i];
                totalSpace = chanceRanges[i + 1].start - chanceRanges[i].start;
                break;
            }
        }
        if(chanceRange.start == -1)
        {
            chanceRange = chanceRanges[chanceRanges.Length - 1];
            totalSpace = 1 - chanceRanges[chanceRanges.Length - 1].start;
        }

        return Mathf.Lerp(chanceRange.startChance, chanceRange.endChance, (progress - chanceRange.start)/totalSpace);
    }


    public GameObject GetEnemy()
    {
        return enemy;
    }

    [System.Serializable]
    struct ChanceRange
    {
        [SerializeField] public float start;
        [SerializeField] public float startChance;
        [SerializeField] public float endChance;
    }
}
