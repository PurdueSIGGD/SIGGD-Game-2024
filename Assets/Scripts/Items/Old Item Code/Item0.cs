using UnityEngine;

public class Item0 : MonoBehaviour
{
    private StatManager stats;
    private bool isactive;
    private int effect;
    // Start is called before the first frame update
    void Start()
    {
        stats = PlayerID.instance.GetComponent<StatManager>();
        effect = 150;
        isactive = false;
    }
    void ItemActivate()
    {
        stats.ModifyStat("Max Health", effect);
        isactive = true;
    }
    void ItemDeactivate()
    {
        stats.ModifyStat("Max Health", -effect);
        isactive = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
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
