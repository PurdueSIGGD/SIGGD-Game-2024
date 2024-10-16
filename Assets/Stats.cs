using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField]
    Stat[] stats;

    [SerializeField]
    float value;

    [SerializeField]

    int modifier;
    [SerializeField]
    public struct Stat {
        public string name;
        public String GetName() {
            return name;
        }
        public int value;
        public int modifier;
    }
    
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ModifyStat(int buff) {
        modifier = modifier + buff;
    }

    public float ComputeValue() {
        float finalValue = value * (modifier / 100f);
        return finalValue;
    }
}
