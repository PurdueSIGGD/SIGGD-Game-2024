using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerID : MonoBehaviour
{
    public static PlayerID instance;

    private void Awake()
    {
        instance = this;
    }
}
