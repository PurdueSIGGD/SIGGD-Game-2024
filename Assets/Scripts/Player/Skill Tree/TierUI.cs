using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierUI : MonoBehaviour
{
    public void Visualize(int unusedPoints)
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(i < unusedPoints);
            i++;
        }
    }
}
