using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item2 : MonoBehaviour
{
    private StatManager stats;
    private bool isactive;
    private int effect;
    // Start is called before the first frame update
    void Start()
    {
        stats = PlayerID.instance.GetComponent<StatManager>();
        effect = 50;
        isactive = false;
    }
    void ItemActivate()
    {
        stats.ModifyStat("Jump Force", effect);
        isactive = true;
    }
    void ItemDeactivate()
    {
        stats.ModifyStat("Jump Force", -effect);
        isactive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (!isactive)
            {
                ItemActivate();
            }
            else
            {
                ItemDeactivate();
            }
        }
    }
}
