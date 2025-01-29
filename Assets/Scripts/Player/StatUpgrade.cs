using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Stats stats;
    [SerializeField]
    private StatBuff[] statsbuff;

    //Dictioanry to check if buff/item is active or not
    private Dictionary<string, bool> activeItems = new Dictionary<string, bool>();

    [Serializable]
    //List of the buffs and the stats
    public struct StatBuff
    {
        public string name;
        public string key; // key pressed to activeate/deactivate the stat
        public string modname; //original stat the buff should modify
        public int buff; //amount the buff changes
    }
    // Start is called before the first frame update
    void Start()
    {
        stats = PlayerID.instance.GetComponent<Stats>();
        foreach (var stBuff in statsbuff)
        {
            activeItems[stBuff.name] = false;
        }//initialize dict

    }

    // Update is called once per frame
    void Update()
    {
        ItemActive();
    }
    void ItemActive()
    {
        for (int i = 0; i < statsbuff.Length; i++)
            {
                if (Input.GetKeyDown(statsbuff[i].key))
                {
                    //check if buff is active or not
                    if (!activeItems[statsbuff[i].name])
                    {   //adds buff to stat modifier
                        stats.ModifyStat(stats.GetStatIndex(statsbuff[i].modname), statsbuff[i].buff);
                        activeItems[statsbuff[i].name] = true;
                        return;
                    }
                    else
                    {
                        //subtracts buff from stat modifier
                        stats.ModifyStat(stats.GetStatIndex(statsbuff[i].modname), -statsbuff[i].buff);
                        activeItems[statsbuff[i].name] = false;
                        return;
                    }
                }
            }
    }
}
