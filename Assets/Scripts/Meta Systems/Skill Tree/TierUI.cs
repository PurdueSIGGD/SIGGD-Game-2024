using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TierUI : MonoBehaviour, IPointerClickHandler
{
    // -- Private Fields --
    private int tier;
    private SkillTree skillTree;

    private void Update()
    {
        if (skillTree != null)
        {
            int unusedPoints = skillTree.GetTierPoints(tier);

            int i = 0;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(i < unusedPoints);
                i++;
            }
        }
    }

    // -- External Functions --
    public void Visualize(SkillTree skillTree, int tier, GameObject ghost)
    {
        this.tier = tier;
        this.skillTree = skillTree;

        foreach (Transform child in transform)
        {
            child.GetComponent<Image>().color = ghost.GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        skillTree.ResetPoints(tier);
    }

}
