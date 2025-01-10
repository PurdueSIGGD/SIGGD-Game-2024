using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayEventHolder : MonoBehaviour
{

    // ON DAMAGE DEALT
    /// <param name="context">Struct containing context for this event.</param>
    public delegate void DamageDealtEvent(DamageContext context);
    /// <summary>
    /// Invoked when the player deals damage.
    /// </summary>
    public static DamageDealtEvent OnDamageDealt;

    // ON DAMAGE RECEIVED
    /// <param name="context">Struct containing context for this event.</param>
    public delegate void DamageReceivedEvent(DamageContext context);
    /// <summary>
    /// Invoked when the player receives damage.
    /// </summary>
    public static DamageReceivedEvent OnDamageReceived;

}
