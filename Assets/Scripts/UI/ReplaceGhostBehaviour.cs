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
        if (ghostsInParty.Count >= 2)
        {
            ghostSlot1.Visualize(ghostsInParty[0], 2);
            ghostSlot2.Visualize(ghostsInParty[1], 3);
        }
        gameObject.SetActive(false);
    }

    public void Choose()
    {
        gameObject.SetActive(true);
        Debug.Log("Choose ghost to replace");
        PlayerID.instance.FreezePlayer();
        PlayerID.instance.FreezePlayerMouse();
    }

    public void Chosen()
    {         
        gameObject.SetActive(false);
        ghostReplacedDelegate.Invoke();
        PlayerID.instance.UnfreezePlayer();
        PlayerID.instance.UnfreezePlayerMouse();
    }
}
