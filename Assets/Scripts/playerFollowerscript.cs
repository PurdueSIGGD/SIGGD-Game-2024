using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFollowerscript : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 1.5f, -13);
    }
}
