using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAllSkills : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Skill[] skills = GetComponents<Skill>();
        foreach (Skill skill in skills) {
            skill.ClearPoints();
        }
    }
}
