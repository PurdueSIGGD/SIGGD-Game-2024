using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceGhostBehaviour : MonoBehaviour
{
    [SerializeField] private GhostSlot ghostSlot1;
    [SerializeField] private GhostSlot ghostSlot2;

    public static ReplaceGhostBehaviour Instance;

    public delegate void GhostReplacedDelegate();
    public GhostReplacedDelegate ghostReplacedDelegate;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        List<GhostIdentity> ghostsInParty = PartyManager.instance.GetGhostPartyList();
        ghostSlot1.Visualize(ghostsInParty[0]);
        ghostSlot2.Visualize(ghostsInParty[1]);
        ghostSlot1.OtherGhost(ghostsInParty[1]);
        ghostSlot2.OtherGhost(ghostsInParty[0]);
        gameObject.SetActive(false);
    }

    public void Choose()
    {
        gameObject.SetActive(true);
        PlayerID.instance.FreezePlayer();
    }

    public void Chosen()
    {         
        gameObject.SetActive(false);
        ghostReplacedDelegate.Invoke();
        PlayerID.instance.UnfreezePlayer();
    }
}
