using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Item1 : MonoBehaviour
{

    private Stats stats;
    private bool isactive;
    private int effect;
    // Start is called before the first frame update
    void Start()
    {
        stats = PlayerID.instance.GetComponent<Stats>();
        effect = 50;
        isactive = false;
    }
    void ItemActivate()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            stats.ModifyStat("Max Running Speed", effect);
            isactive = true;
        }
    }
    void ItemDeactivate()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            stats.ModifyStat("Max Running Speed", -effect);
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
}
