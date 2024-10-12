using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    String statName;

    [SerializeField]
    float value;

    [SerializeField]

    int modifier;


    
    // Start is called before the first frame update
    public String GetName() {
        return statName;
    }
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
