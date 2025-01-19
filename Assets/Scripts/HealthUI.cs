using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI healthText;
    private Health health;
    private Quaternion UIRotation;

    // Start is called before the first frame update
    void Start()
    {
        health = transform.parent.GetComponent<Health>();
        UIRotation = new Quaternion(0f, transform.parent.rotation.y, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        UIRotation.y = transform.parent.rotation.y;
        transform.localRotation = UIRotation;
        //Debug.Log("UIRotation.y: " + UIRotation.y + "   |   transform.parent.rotation.y: " + transform.parent.rotation.y + "   |   transform.localRotation.y: " + transform.localRotation.y);
        healthText.text = health.currentHealth + " | " + health.maxHealth;
    }
}
