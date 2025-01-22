using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI healthText;
    private Health health;
    private Stats stats;
    private Quaternion UIRotation;

    // Start is called before the first frame update
    void Start()
    {
        health = transform.parent.GetComponent<Health>();
        stats = transform.parent.GetComponent<Stats>();
        UIRotation = new Quaternion(0f, transform.parent.rotation.y, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        UIRotation.y = transform.parent.rotation.y;
        transform.localRotation = UIRotation;
        healthText.text = Mathf.CeilToInt(health.currentHealth) + " | " + Mathf.CeilToInt(stats.ComputeValue("Max Health"));
    }
}
