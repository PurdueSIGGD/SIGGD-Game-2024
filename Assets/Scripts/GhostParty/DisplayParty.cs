using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayParty : MonoBehaviour
{
    public PartyManager player;
    public TextMeshProUGUI display;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PartyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        display.text = "Major Ghosts in Party [B]: ";
        for (int i = 0; i < player.GetGhostMajorList().Count; i++) {
            display.text += player.GetGhostMajorList()[i].getName();
            if (i < player.GetGhostMajorList().Count - 1) {
                display.text += ", ";
            }
        }
    }
}
