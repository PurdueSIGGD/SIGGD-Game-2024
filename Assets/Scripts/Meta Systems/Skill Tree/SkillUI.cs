using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour, IPointerClickHandler
{
    // -- Serialize Fields --
    [Header("References")]
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI desc;
    [SerializeField] TextMeshProUGUI descVal;
    [SerializeField] Image[] skillPoints;

    [Header("Values")]
    [SerializeField] Color emptyPoint;
    [SerializeField] Color fillPoint;

    // -- Private Variables --
    private Skill skill;
    private SkillTree skillTree;
    // -- Internal Functions --
    private void Update()
    {
        if (skill != null)
        {
            for (int i = 0; i < skillPoints.Length; i++)
            {
                if (i < skill.GetPoints())
                {
                    skillPoints[i].color = fillPoint;
                }
                else
                {
                    skillPoints[i].color = emptyPoint;
                }
            }
        }
    }

    // -- External Functions --

    public void Visualize(SkillTree skillTree, Skill skill)
    {
        this.skill = skill;
        this.skillTree = skillTree;

        title.text = skill.GetName();
        desc.text = skill.GetDescription();
        icon.sprite = skill.GetIcon();
        descVal.text = skill.GetDescriptionValue();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        skillTree.TryAddPoint(skill);
    }
}
