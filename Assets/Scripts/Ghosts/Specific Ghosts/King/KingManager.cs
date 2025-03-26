using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingManager : GhostManager, ISelectable
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }



    public override void Select(GameObject player)
    {
        Debug.Log("KING SELECTED");
        base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        base.DeSelect(player);
    }
}
