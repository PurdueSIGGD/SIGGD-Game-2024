using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
    private SkillTreeUI skillTreeUI;
    private int onPanelFrameCount = 0;
    private bool isHovered;
    private Animator animator;

    // -- Internal Functions --
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isHovered)
        {
            onPanelFrameCount += 1;
        }

        if (onPanelFrameCount > 200)
        {
            //animator.Play("SkillFocused");
        }
    }

    // -- External Functions --
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        onPanelFrameCount = 0;
        //animator.Play("SkillUnfocused");
    }

    public void Visualize(Skill skill, SkillTreeUI skillTreeUI)
    {
        this.skill = skill;
        this.skillTreeUI = skillTreeUI;

        title.text = skill.GetName();
        desc.text = skill.GetDescription();
        icon.sprite = skill.GetIcon();
        descVal.text = skill.GetDescriptionValue();

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
        //animator.Play("SkillFocused");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        skillTreeUI.TryAddPointUI(this.skill);
    }
}
