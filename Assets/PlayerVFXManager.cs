using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerVFXManager : MonoBehaviour
{

    [DoNotSerialize] public static PlayerVFXManager instance;

    [SerializeField] public Animator lightAttack1;
    [SerializeField] public Animator lightAttack2;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    
}
