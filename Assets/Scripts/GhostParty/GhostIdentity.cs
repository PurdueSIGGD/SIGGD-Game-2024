using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowEntity))]
[RequireComponent(typeof(GhostBuff))]
public class GhostIdentity : MonoBehaviour
{
    [SerializeField]
    private string ghostName;

    private bool inParty = false;
    private GameObject player;
    private FollowEntity followScript;
    private GhostBuff buffScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        followScript = this.GetComponent<FollowEntity>();
        buffScript = this.GetComponent<GhostBuff>();
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
            this.followScript.ChangeTarget(player);
            this.buffScript.EnterParty(player);
        } else
        {
            this.followScript.ChangeTarget(null);
            this.buffScript.ExitParty(player);
        }
    }

}
