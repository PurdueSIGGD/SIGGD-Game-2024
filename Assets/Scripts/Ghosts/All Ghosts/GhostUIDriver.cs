using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostUIDriver : MonoBehaviour, ISelectable
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

    public virtual void Select(GameObject player)
    {

    }

    public virtual void DeSelect(GameObject player)
    {
        meterUIManager.deactivateWidget();
    }

}
