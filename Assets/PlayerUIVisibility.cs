using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIVisibility : MonoBehaviour
{
    public static PlayerUIVisibility instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ShowPlayerUI() 
    {
        PlayerSelectedGhostUIManager.instance.gameObject.SetActive(true);
        if (PartyManager.instance.GetGhostPartyList().Count >= 1)
            PlayerGhost1UIManager.instance.gameObject.SetActive(true);
        if (PartyManager.instance.GetGhostPartyList().Count >= 2)
            PlayerGhost2UIManager.instance.gameObject.SetActive(true);
        if (SpiritTrackerCanvasUI.Instance != null)
            SpiritTrackerCanvasUI.Instance.gameObject.SetActive(true);
    }

    public void HidePlayerUI()
    {
        PlayerSelectedGhostUIManager.instance.gameObject.SetActive(false);
        PlayerGhost1UIManager.instance.gameObject.SetActive(false);
        PlayerGhost2UIManager.instance.gameObject.SetActive(false);
        if (SpiritTrackerCanvasUI.Instance != null)
            SpiritTrackerCanvasUI.Instance.gameObject.SetActive(false);
    }
}
