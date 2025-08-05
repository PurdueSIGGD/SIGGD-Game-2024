using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SpiritShopManager : MonoBehaviour, IScreenUI
{

    // ==============================
    //       Serialized Fields
    // ==============================
    [SerializeField] private TMP_Text redSpiritCountText;
    [SerializeField] private TMP_Text blueSpiritCountText;
    [SerializeField] private TMP_Text yellowSpiritCountText;


    [SerializeField] private ItemUIManager itemBox1;
    [SerializeField] private ItemUIManager itemBox2;
    [SerializeField] private ItemUIManager itemBox3;

    // ==============================
    //        Other Variables
    // ==============================

    private const int NUM_ITEMS = 3; // num items to display in shop 

    public void OnNextCloseCall(UnityAction action)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        //redSpiritCountTexst.text =

        //itemBox1.createItemBox("Test 1");
        //itemBox2.createItemBox("Test 2");
        //itemBox3.createItemBox("Test 3");




    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
