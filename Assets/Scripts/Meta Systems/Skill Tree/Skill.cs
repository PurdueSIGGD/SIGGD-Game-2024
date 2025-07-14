using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Skill : MonoBehaviour
{
    [SerializeField]
    protected SkillSO skillSO;
    protected int skillIndex;
    protected string identityName;
    protected GhostIdentity identity;

    private void Awake()
    {
        identityName = name;
        /*Skill[] skills = GetComponent<SkillTree>().GetAllSkills();
        for(int i = 0; i <skills.Length; i++)
        {
            if (skills[i] == this)
            {
                skillIndex = i;
            }
        }
        */

        if (identityName.Contains("(Clone)"))
        {
            identityName = identityName.Replace("(Clone)", "");
        }

        if (!SaveManager.data.ghostSkillPts.ContainsKey(identityName))
        {
            SaveManager.data.ghostSkillPts.Add(identityName, new int[7]);
        }

        for (int i = 0; i < SaveManager.data.ghostSkillPts[identityName][skillIndex]; i++)
        {
            AddPointTrigger();
        }
        
    }

    public void AddPoint()
    {
        SaveManager.data.ghostSkillPts[identityName][skillIndex]++;
        AddPointTrigger();
    }
    public void RemovePoint()
    {
        SaveManager.data.ghostSkillPts[identityName][skillIndex]--;
        RemovePointTrigger();
    }
    public void ClearPoints()
    {
        SaveManager.data.ghostSkillPts[identityName][skillIndex] = 0;
        ClearPointsTrigger();
    }

    public abstract void AddPointTrigger();
    public abstract void RemovePointTrigger();
    public abstract void ClearPointsTrigger();

    public int GetPoints()
    {
        return SaveManager.data.ghostSkillPts[identityName][skillIndex];
    }

    public string GetName()
    {
        return skillSO.skillName;
    }

    public string GetDescription()
    {
        return skillSO.description;
    }

    public Sprite GetIcon()
    {
        return skillSO.sprite;
    }

    public string GetDescriptionValue()
    {
        return skillSO.descriptionValue;
    }

    public void SetSkillIndex(int ind)
    {
        skillIndex = ind;
    }
}
