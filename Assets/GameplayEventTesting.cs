using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayEventTesting : MonoBehaviour
{

    private void OnEnable()
    {
        //GameplayEventManager.OnDamageReceived += damageReceivedDebug;
        GameplayEventHolder.OnDamageReceived += damageReceivedDebug;
        //GameplayEventManager.OnLethalDamageReceived += lethalDamageReceivedDebug;
    }

    private void OnDisable()
    {
        //GameplayEventManager.OnDamageReceived -= damageReceivedDebug;
        GameplayEventHolder.OnDamageReceived -= damageReceivedDebug;
        //GameplayEventManager.OnLethalDamageReceived -= lethalDamageReceivedDebug;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    private void damageReceivedDebug(float damage, GameObject attacker, GameObject victim, string context)
    {
        Debug.Log("Damage Received: " + damage + " Attacker: " + attacker.name + " Victim: " + victim.name + " Context: " + context);
    }
    */

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

    private void lethalDamageReceivedDebug(float damage, GameObject attacker, GameObject victim, string context)
    {
        Debug.Log("Damage Received: " + damage + " Attacker: " + attacker.name + " Victim: " + victim.name + " Context: " + context);
    }
}
