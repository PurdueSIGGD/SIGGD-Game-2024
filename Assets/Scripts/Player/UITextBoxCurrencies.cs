using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UITextBoxCurrencies : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField]
    int currency_type;

    public Text currencyText;
    
    public void OnTextPress() {
        if (currency_type == 1) {
            SpiritCurrencyTracker.blue_spirits++;
        }
        else if (currency_type == 2) {
            SpiritCurrencyTracker.red_spirits++;
        }
        else if (currency_type == 3) {
            SpiritCurrencyTracker.purple_spirits++;
        }
        else if (currency_type == 4) {
            SpiritCurrencyTracker.yellow_spirits++;
        }
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
