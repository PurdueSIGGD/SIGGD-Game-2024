using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthSlider;

    private Health health;
    private StatManager stats;

    // Start is called before the first frame update
    void Start()
    {
        health = PlayerID.instance.GetComponent<Health>();
        stats = PlayerID.instance.GetComponent<StatManager>();

        healthSlider.maxValue = stats.ComputeValue("Max Health");
        healthSlider.value = healthSlider.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        updateHealthWidget();
    }

    private void updateHealthWidget()
    {
        healthText.text = Mathf.CeilToInt(health.currentHealth) + " | " + Mathf.CeilToInt(stats.ComputeValue("Max Health"));
        healthSlider.value = health.currentHealth;
    }
}
