using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostIdentity : MonoBehaviour
{
    [SerializeField]
    private string name;

    private bool inParty = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getName() {
        return name;
    }

    public bool isInParty() {
        return inParty;
    }

    public void setInParty(bool inParty) {
        this.inParty = inParty;
    }

}
