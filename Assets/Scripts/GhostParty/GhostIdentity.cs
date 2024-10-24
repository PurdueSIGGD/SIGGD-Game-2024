using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostIdentity : MonoBehaviour
{
    [SerializeField]
    private string ghostName;

    private bool inParty = false;
    private GameObject player;
    private IParty[] partyScripts;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        partyScripts = this.GetComponents<IParty>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getName() {
        return ghostName;
    }

    public bool isInParty() {
        return inParty;
    }

    public void setInParty(bool inParty) {
        this.inParty = inParty;
        if (inParty)
        {
            foreach(IParty script in partyScripts)
            {
                script.EnterParty(player);
            }
        } else
        {
            foreach (IParty script in partyScripts)
            {
                script.ExitParty(player);
            }
        }
    }

}
