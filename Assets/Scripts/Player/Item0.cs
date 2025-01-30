using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Stats stats;
    private bool isactive;
    private int effect;
    // Start is called before the first frame update
    void Start()
    {
        stats = PlayerID.instance.GetComponent<Stats>();
        effect = 150;
        isactive = false;
    }
    void ItemActivate()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            stats.ModifyStat("Max Health", effect);
            isactive = true;
        }
    }
    void ItemDeactivate()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            stats.ModifyStat("Max Health", -effect);
            isactive = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isactive)
        {
            ItemDeactivate();
        }
        else
        {
            ItemActivate();
        }
    }
  /*  void ItemActive()
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
    }*/
}
