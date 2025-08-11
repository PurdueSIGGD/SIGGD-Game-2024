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

    [Header("Item Boxes")]

    [SerializeField] private GameObject redItemBox;
    [SerializeField] private GameObject blueItemBox;
    [SerializeField] private GameObject yellowItemBox;

    [Header("UI")]

    [SerializeField] private SpiritCounterUI redCounterUI;
    [SerializeField] private SpiritCounterUI blueCounterUI;
    [SerializeField] private SpiritCounterUI yellowCounterUI;

    [SerializeField] private Button secureSpiritsButton;
    [SerializeField] private TMP_Text secureSpiritsButtonText;

    // ==============================
    //        Other Variables
    // ==============================

    private const int NUM_ITEMS = 3; // num items to display in shop 

    private SpiritTracker spiritTracker;
    private bool turnCompleted = false; // Whether user completed an action (buy item or secure)

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

        redItemBox.GetComponent<ItemUIManager>().PickAndDisplayRandomItem();
        blueItemBox.GetComponent<ItemUIManager>().PickAndDisplayRandomItem();
        yellowItemBox.GetComponent<ItemUIManager>().PickAndDisplayRandomItem();

        gameObject.SetActive(true);
    }

    public void CloseShopUI()
    {
        Door.activateDoor(true);
        gameObject.SetActive(false);
    }

    public void UpdateSpiritCountText()
    {
        redCounterUI.UpdateText();
        blueCounterUI.UpdateText();
        yellowCounterUI.UpdateText();
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
