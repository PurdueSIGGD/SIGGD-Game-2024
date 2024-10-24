using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGhost : MonoBehaviour
{
    [SerializeField] GameObject ghost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSwitchPartyStatus()
    {
        ghost.GetComponent<GhostBuff>().SwitchPartyStatus(this.gameObject);
    }
}
