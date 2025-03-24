using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GhostSkillUIBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Skill skill;
    [SerializeField] Image icon;
    [SerializeField] Image[] skillPoints;
    [SerializeField] Sprite filled;
    [SerializeField] Sprite empty;
    [SerializeField] Sprite overcharged;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI desc;
    [SerializeField] Vector2 offset;
    [SerializeField] GhostSkillTreeUIBehaviour skillTree;

    private int onPanel = 0;
    private bool hovering;

    private void Start()
    {
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
            desc.gameObject.SetActive(false);
        }

        if(onPanel > 200)
        {
            desc.gameObject.SetActive(true);
            desc.gameObject.transform.position = Input.mousePosition + new Vector3(offset.x, offset.y, 0);
            desc.text = skill.GetDescription();
        }

        if (Input.GetMouseButtonDown(0) && hovering)
        {
            Skill skill = skillTree.AddSkillPointToSkill(this.skill, 1);
            if (skill != null)
            {
                Visualize(skill);
                this.skill = skill;
            }
        }
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
        for (int i = 0; i < skillPoints.Length; i++)
        {
            skillPoints[i].sprite = empty;
        }
        icon.sprite = skill.GetSprite();
        int skillPts = skill.GetPts();
        for (int i = 0; i < skillPts; i++)
        {
            skillPoints[i].sprite = filled;
        }
        desc.text = skill.GetDescription();
        title.text = skill.GetName();
        this.skill = skill;
    }
}
