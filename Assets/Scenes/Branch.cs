using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    private list<Skill> skillList;
    // Start is called before the first frame update
    void Start()
    {
        skillList = new list<Skill>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Skill getNextSkill(Skill skill) {
        return skillList[skillList.indexOf(skill) + 1];
    }

    public Skill getNextLockedSkill() {
        foreach(Skill s in skillList) {
            if (!s.getUnlocked()) {
                return s;
            }
        }
        return null;
    }

    public void unlockSkill(Skill skill) {
        skill.setUnoock(true);
    }
}
