#define DEBUG_LOG

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

    [SerializeField] private int rerollStartPrice;
    [SerializeField] private int rerollPriceIncrement;

    [Header("Item Boxes")]

    [SerializeField] private GameObject redItemBox;
    [SerializeField] private GameObject blueItemBox;
    [SerializeField] private GameObject yellowItemBox;

    [Header("UI")]

    [SerializeField] private TMP_Text redSpiritCountText;
    [SerializeField] private TMP_Text blueSpiritCountText;
    [SerializeField] private TMP_Text yellowSpiritCountText;

    [SerializeField] private Button secureSpiritsButton;
    [SerializeField] private TMP_Text secureSpiritsButtonText;

    [SerializeField] private Button rerollButton;
    [SerializeField] private TMP_Text rerollButtonText;

    // ==============================
    //        Other Variables
    // ==============================

    private const int NUM_ITEMS = 3; // num items to display in shop 

    private SpiritTracker spiritTracker;
    private bool turnCompleted = false;

    public void OnNextCloseCall(UnityAction action)
    {
        Debug.Log("on next close call");
    }

    // Start is called before the first frame update
    void Start()
    {

        spiritTracker = PersistentData.Instance.GetComponent<SpiritTracker>();

        secureSpiritsButton.onClick.AddListener(SecureSpirits);

        gameObject.SetActive(false);
        
    }

    public void OpenShopUI()
    {
        if (turnCompleted) return;

        redItemBox.GetComponent<ItemUIManager>().DisplayRandomItem();
        blueItemBox.GetComponent<ItemUIManager>().DisplayRandomItem();
        yellowItemBox.GetComponent<ItemUIManager>().DisplayRandomItem();


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
        int redSpirits = spiritTracker.redSpiritsCollected;
        int blueSpirits = spiritTracker.blueSpiritsCollected;
        int yellowSpirits = spiritTracker.yellowSpiritsCollected;

        redSpiritCountText.text = redSpirits.ToString();
        blueSpiritCountText.text = blueSpirits.ToString();
        yellowSpiritCountText.text = yellowSpirits.ToString();

        secureSpiritsButtonText.text = "Secure " + (redSpirits + blueSpirits + yellowSpirits);
    }

    /// <summary>
    /// Save spirits to save data
    /// </summary>
    private void SecureSpirits()
    {
        spiritTracker.SaveSpiritCounts();

#if DEBUG_LOG
        Debug.Log("saved spirits");
#endif

        turnCompleted = true;

        CloseShopUI();
    }
}
