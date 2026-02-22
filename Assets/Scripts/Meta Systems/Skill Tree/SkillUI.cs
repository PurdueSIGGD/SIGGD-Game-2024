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
    [SerializeField] TextMeshProUGUI descLevel;
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
        if (skill != null && skillTree != null)
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

            if (descLevel == null) return;
            string currLevelValue = skill.GetDescriptionLevelValue(skill.GetPoints());
            bool canSeeLevel4 = (skillTree.GetLevel() > 10);
            string descLevelConnector = ((canSeeLevel4 && skill.GetPoints() >= 4) || (!canSeeLevel4 && skill.GetPoints() >= 3)) ? (" <size=85%>max lvl</size>") : (" <i>-></i> ");
            string nextLevelValue = ((canSeeLevel4 && skill.GetPoints() >= 4) || (!canSeeLevel4 && skill.GetPoints() >= 3)) ? ("") : (skill.GetDescriptionLevelValue(skill.GetPoints() + 1));
            descLevel.text = "<size=110%>" + currLevelValue + "</size><color=#000000E0>" + descLevelConnector + "<size=85%>" + nextLevelValue + "</size></color>";
        }
    }

    // -- External Functions --

    public void Visualize(SkillTree skillTree, Skill skill, GameObject ghost)
    {
        this.skill = skill;
        this.skillTree = skillTree;

        title.text = skill.GetName();
        desc.text = skill.GetDescription();
        icon.sprite = skill.GetIcon();
        descVal.text = skill.GetDescriptionValue();

        CharacterSO ghostInfo = ghost.GetComponent<GhostIdentity>().GetCharacterInfo();
        icon.color = ghostInfo.primaryColor;
        fillPoint = ghostInfo.highlightColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        skillTree.TryAddPoint(skill);
    }
}
