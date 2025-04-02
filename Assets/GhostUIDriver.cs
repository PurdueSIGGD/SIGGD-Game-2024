using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostUIDriver : MonoBehaviour
{
    protected StatManager stats;
    protected GhostIdentity ghostIdentity;
    protected PlayerInWorldMeterUIManager meterUIManager;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        stats = GetComponent<StatManager>();
        ghostIdentity = GetComponent<GhostIdentity>();
        meterUIManager = PlayerInWorldMeterUIManager.instance;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

}
