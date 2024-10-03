using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private string name;
    private int cost;
    private bool unlocked; 

    public Skill(string n, int c) {
        name = n;
        cost = c;
        unlocked = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setUnoock(bool state) {
        unlocked = state;
    }

    public bool getUnlocked() {
        return unlocked;
    }
}
