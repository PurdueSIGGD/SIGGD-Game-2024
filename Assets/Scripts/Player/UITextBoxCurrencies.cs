using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UITextBoxCurrencies : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameObject player;

    public UITextBoxCurrencies() {
        player = GameObject.FindWithTag("Player");
    }
    private static SpiritCurrencyTracker SpiritTracker =  player.GetComponent<SpiritCurrencyTracker>();
    SpiritTypes spiritTypes = SpiritTracker.SpiritTypes;
    private int blue_currency = (int) SpiritTracker.SpiritTypes.Blue;
    private int red_currency = player.GetComponent<SpiritCurrencyTracker>().Red;
    private int purple_currency = player.GetComponent<SpiritCurrencyTracker>().Purple;
    private int yellow_currency = player.GetComponent<SpiritCurrencyTracker>().Yellow;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
