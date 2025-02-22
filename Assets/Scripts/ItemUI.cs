using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemUI : MonoBehaviour
{

    private bool IsBought = false;
    private bool IsSelected = false;
    [SerializeField] public TextMeshProUGUI buttonText;
    [SerializeField] public TextMeshProUGUI itemText;



    // Start is called before the first frame update
    void Start()
    {

    }

    public void onClick() {

        if (IsBought) {

            if (IsSelected) {
                IsSelected = false;
            }
            else {
                IsSelected = true;
            }
            if (IsSelected) {
                buttonText.text = "Deselect";
            }
            else{
                buttonText.text = "Select";
            } 
        }
        else {
            IsBought = true;
            buttonText.text = "Select";
        }
        
    }

    public void setItemName(string name) {
        itemText.text = name;
    }
}
