using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerBuffStats : MonoBehaviour, IStatList
{
    [SerializeField]
    public StatManager.Stat[] statList;
    protected StatManager stats;


    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<StatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //Stuff



    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }

    public StatManager GetStats()
    {
        return stats;
    }
}
