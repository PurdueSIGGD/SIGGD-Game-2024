using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyAttackBehaviour : MonoBehaviour
{
    [SerializeField] GameObject heavyIndicator;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            heavyIndicator.SetActive(false);
        }
    }

    void OnHeavy()
    {
        heavyIndicator.SetActive(true);
        timer = 0.5f;
    }
}
