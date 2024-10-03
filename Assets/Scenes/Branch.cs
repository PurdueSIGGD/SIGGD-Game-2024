using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    private List<Skill> skillList;
    // Start is called before the first frame update
    void Start()
    {
        skillList = new List<Skill>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Skill GetNextSkill(Skill skill) {
        return skillList[skillList.IndexOf(skill) + 1];
    }

    public Skill GeetNextLockedSkill() {
        foreach(Skill s in skillList) {
            if (!s.GetUnlocked()) {
                return s;
            }
        }
        return null;
    }

    public void UnlockSkill(Skill skill) {
        skill.SetUnlock(true);
    }
}
