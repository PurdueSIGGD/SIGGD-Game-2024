using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SkillUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI desc;
    [SerializeField] TextMeshProUGUI descVal;
    [SerializeField] Image[] skillPoints;

    private Skill skill;
    private int onPanelFrameCount = 0;
    private bool isHovered;
    private Animator animator;

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
            animator.Play("SkillFocused");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        onPanelFrameCount = 0;
        animator.Play("SkillUnfocused");
    }

    public void Visualize(Skill skill)
    {
        this.skill = skill;
        title.text = skill.GetName();
        desc.text = skill.GetDescription();
        icon.sprite = skill.GetIcon();
        descVal.text = skill.GetDescriptionValue();

        for (int i = 0; i < skillPoints.Length; i++)
        {
            if (i < skill.GetPoints())
            {
                skillPoints[i].color = Color.black;
            } else
            {
                skillPoints[i].color = Color.white;
            }
        }
    }

    public Skill GetAssociatedSkill()
    {
        return skill;
    }
}
