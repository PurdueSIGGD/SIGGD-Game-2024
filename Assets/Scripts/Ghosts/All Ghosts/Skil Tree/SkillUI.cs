using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SkillUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Skill skill;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI desc;
    [SerializeField] Image[] skillPoints;

    private int onPanel = 0;
    private bool hovering;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Visualize(skill);
    }

    private void Update()
    {
        if (hovering)
        {
            onPanel += 1;
        }
        else
        {
            onPanel = 0;
            animator.Play("SkillUnfocused");
        }

        if (onPanel > 200)
        {
            animator.Play("SkillFocused");
        }

        /*if (Input.GetMouseButtonDown(0) && hovering)
        {
            Skill skill = skillTree.AddSkillPointToSkill(this.skill, 1);
            if (skill != null)
            {
                Visualize(skill);
                this.skill = skill;
            }
        }*/
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }

    public void Visualize(Skill skill)
    {
        this.skill = skill;
        title.text = skill.GetName();
        desc.text = skill.GetDescription();

        for (int i = 0; i < skillPoints.Length; i++)
        {
            if (i < skill.GetPoints())
            {
                skillPoints[i].color = Color.red;
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
