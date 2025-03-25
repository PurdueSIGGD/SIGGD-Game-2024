using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TierUI : MonoBehaviour, IPointerClickHandler
{
    // -- Private Fields --
    private int tier;
    private SkillTreeUI skillTreeUI;


    // -- External Functions --
    public void Visualize(int tier, SkillTreeUI skillTreeUI)
    {
        this.tier = tier;
        this.skillTreeUI = skillTreeUI;

        int unusedPoints = skillTreeUI.GetSkillTree().GetTierPoints(tier);

        int i = 0;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(i < unusedPoints);
            i++;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        skillTreeUI.ResetTierPointsUI(tier);
    }

}
