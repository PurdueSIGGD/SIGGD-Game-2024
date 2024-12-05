using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerID : MonoBehaviour
{
    public static PlayerID instance;

    private void Awake()
    {
        instance = this;
    }
}
