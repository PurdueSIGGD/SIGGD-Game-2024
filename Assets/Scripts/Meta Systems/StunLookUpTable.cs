using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Table for looking up different stun values based on attack strengths.
/// This is only used for hit-stuns, which is the minor flinching caused by player attacks
/// and not actual, long-duration stuns.
/// </summary>
[DisallowMultipleComponent]
public class StunLookUpTable : MonoBehaviour
{
    public static Hashtable table = new Hashtable();

    private void Awake()
    {
        // For reference, a standard enemy like Riot Police might have 100 stun threshold
        // A weaker/long range enemy might have a stun threshold of 40.

        table.Add(DamageStrength.MEAGER, 0f); // a meager attack will never build up stun or stop stun decay
        table.Add(DamageStrength.LIGHT, 10f); // a light attack should build up negligible stun
        table.Add(DamageStrength.MODERATE, 35f); // repeated moderate attacks should consistently cause hit stun (ex. player light attack)
        table.Add(DamageStrength.HEAVY, 70f); // a heavy attack should build up considerable stun
        table.Add(DamageStrength.DEVASTATING, 200f); // a devastating attack should consistently hit stun an enemy
    }
}
