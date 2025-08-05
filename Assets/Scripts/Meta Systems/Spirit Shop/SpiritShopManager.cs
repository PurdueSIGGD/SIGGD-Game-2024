using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpiritShopManager : MonoBehaviour, IScreenUI
{

    // ==============================
    //       Serialized Fields
    // ==============================
    [SerializeField] private TMP_Text redSpiritCountText;
    [SerializeField] private TMP_Text blueSpiritCountText;
    [SerializeField] private TMP_Text yellowSpiritCountText;

    [SerializeField] private Button sendSpiritsToHubButton;

    // ==============================
    //        Other Variables
    // ==============================

    private const int NUM_ITEMS = 3; // num items to display in shop 

    private SpiritTracker spiritTracker;
    private bool turnCompleted = false;

    public void OnNextCloseCall(UnityAction action)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {

        spiritTracker = PersistentData.Instance.GetComponent<SpiritTracker>();

        sendSpiritsToHubButton.onClick.AddListener(SaveSpirits);

        //itemBox1.createItemBox("Test 1");
        //itemBox2.createItemBox("Test 2");
        //itemBox3.createItemBox("Test 3");

        gameObject.SetActive(false);
        


    }

    public void OpenShopUI()
    {
        if (turnCompleted) return;
        UpdateSpiritCountText();
        gameObject.SetActive(true);
    }

    public void CloseShopUI()
    {
        Door.activateDoor(true);
        gameObject.SetActive(false);
    }
    private void UpdateSpiritCountText()
    {
        redSpiritCountText.text = spiritTracker.redSpiritsCollected.ToString();
        blueSpiritCountText.text = spiritTracker.blueSpiritsCollected.ToString();
        yellowSpiritCountText.text = spiritTracker.yellowSpiritsCollected.ToString();
    }

    private void SaveSpirits()
    {
        spiritTracker.SaveSpiritCounts();

        Debug.Log("saved spirits");
        turnCompleted = true;

        CloseShopUI();
    }
}
