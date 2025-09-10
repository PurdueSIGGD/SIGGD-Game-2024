using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quicksilver : Skill
{
    [SerializeField]
    List<int> values = new List<int>
    {
        0, 1, 2, 3, 4
    };
    public int pointIndex;

    private int buffValue = 0;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
        GetComponent<StatManager>().ModifyStat("Blight Max Quicksilver Damage Percent", values[pointIndex] - buffValue);
        buffValue = values[pointIndex];
    }

    public override void ClearPointsTrigger()
    {
        pointIndex = GetPoints();
        GetComponent<StatManager>().ModifyStat("Blight Max Quicksilver Damage Percent", values[pointIndex] - buffValue);
        buffValue = values[pointIndex];
    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
        GetComponent<StatManager>().ModifyStat("Blight Max Quicksilver Damage Percent", values[pointIndex] - buffValue);
        buffValue = values[pointIndex];
    }
}
