using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image healthBar;

    private PlayerHealth health;
    private StatManager stats;

    private float thresholdOne;
    private float thresholdTwo;

    void Start()
    {
        health = PlayerID.instance.GetComponent<PlayerHealth>();
        stats = health.GetStats();

        healthSlider.maxValue = stats.ComputeValue("Max Health");
        healthSlider.value = healthSlider.maxValue;
    }

    void Update()
    {
        UpdateHealthWidget();
    }

    private void UpdateHealthWidget()
    {
        healthText.text = Mathf.CeilToInt(health.currentHealth) + " | " + Mathf.CeilToInt(stats.ComputeValue("Max Health"));
        healthSlider.value = health.currentHealth;
        healthSlider.maxValue = stats.ComputeValue("Max Health");

        if (health.Wounded)
        {
            healthBar.color = Color.yellow;
        }
        else if (health.MortallyWounded)
        {
            healthBar.color = Color.red;
        }
        else
        {
            healthBar.color = new Color(220, 255, 255);
        }
    }
}
