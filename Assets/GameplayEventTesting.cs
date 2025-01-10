using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayEventTesting : MonoBehaviour
{

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += damageDealtDebug;
        GameplayEventHolder.OnDamageReceived += damageReceivedDebug;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= damageDealtDebug;
        GameplayEventHolder.OnDamageReceived -= damageReceivedDebug;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void damageDealtDebug(DamageContext context)
    {
        if (context.Equals(default(DamageContext)))
        {
            Debug.Log("DAMAGE DEALT\n" + "No context provided.");
            return;
        }
        string damageTypes = "";
        foreach (DamageType type in context.damageTypes)
        {
            damageTypes += type + ", ";
        }
        string actionTypes = "";
        foreach (ActionType type in context.actionTypes)
        {
            actionTypes += type + ", ";
        }
        Debug.Log("DAMAGE DEALT\n" +
                  "Attacker: " + context.attacker + "\n" +
                  "Victim: " + context.victim + "\n" +
                  "Damage: " + context.damage + "\n" +
                  "True Damage: " + context.trueDamage + "\n" +
                  "Damage Strength: " + context.damageStrength + "\n" +
                  "Damage Types: " + damageTypes + "\n" +
                  "Ability ID: " + context.actionID + "\n" +
                  "Ability Types: " + actionTypes + "\n" +
                  "Invoking Script: " + context.invokingScript + "\n" +
                  "Extra Context: " + context.extraContext);
    }



    private void damageReceivedDebug(DamageContext context)
    {
        if (context.Equals(default(DamageContext)))
        {
            Debug.Log("DAMAGE RECEIVED\n" + "No context provided.");
            return;
        }
        string damageTypes = "";
        foreach (DamageType type in context.damageTypes)
        {
            damageTypes += type + ", ";
        }
        string actionTypes = "";
        foreach (ActionType type in context.actionTypes)
        {
            actionTypes += type + ", ";
        }
        Debug.Log("DAMAGE RECEIVED\n" +
                  "Attacker: " + context.attacker + "\n" +
                  "Victim: " + context.victim + "\n" +
                  "Damage: " + context.damage + "\n" +
                  "True Damage: " + context.trueDamage + "\n" +
                  "Damage Strength: " + context.damageStrength + "\n" +
                  "Damage Types: " + damageTypes + "\n" +
                  "Ability ID: " + context.actionID + "\n" +
                  "Ability Types: " + actionTypes + "\n" +
                  "Invoking Script: " + context.invokingScript + "\n" +
                  "Extra Context: " + context.extraContext);
    }

}
