using JetBrains.Annotations;
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

    public void OnNextCloseCall(UnityAction action)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {

        spiritTracker = PersistentData.Instance.GetComponent<SpiritTracker>();

        redSpiritCountText.text = spiritTracker.redSpiritsCollected.ToString();
        blueSpiritCountText.text = spiritTracker.blueSpiritsCollected.ToString();
        yellowSpiritCountText.text = spiritTracker.yellowSpiritsCollected.ToString();

        //sendSpiritsToHubButton.onClick.AddListener(SpiritTracker.SaveSpiritCounts);

        //itemBox1.createItemBox("Test 1");
        //itemBox2.createItemBox("Test 2");
        //itemBox3.createItemBox("Test 3");

        gameObject.SetActive(false);
        


    }

    public void OpenShopUI()
    {
        gameObject.SetActive(true);
    }
}
