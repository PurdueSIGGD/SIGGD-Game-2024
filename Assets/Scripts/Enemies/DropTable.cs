using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTable : MonoBehaviour
{
    [SerializeField]
    public List<Drop> dropTable = new List<Drop>();

    [Serializable]
    public class Drop
    {
        [SerializeField] public GameObject obj;
        [SerializeField] public float chance;
        [SerializeField] public float minCount;
        [SerializeField] public float maxCount;
    }
}
