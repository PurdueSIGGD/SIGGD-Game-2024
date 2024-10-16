using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField]
    Stat[] stats;


    [Serializable]
    public struct Stat {
        public string name;
        public int value;
        public int modifier;
    }
    private string name;
    private int value;
    private int modifier;

    public string GetName() {
        return name;
    }
    public int GetValue() {
        return value;
    }
    public int getModifier() {
        return modifier;
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
