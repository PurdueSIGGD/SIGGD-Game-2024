using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeamstressBasicSpiritLoom : MonoBehaviour
{

    private PlayerStateMachine playerStateMachine;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
