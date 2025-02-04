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
    private int trust;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        partyScripts = this.GetComponents<IParty>();
        trust = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetName() {
        return ghostName;
    }

    public bool IsInParty() {
        return inParty;
    }

    public void SetInParty(bool inParty) {
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

    public void addTrust(int amount) {
        trust += amount;
        Debug.Log(trust);
    }

}
