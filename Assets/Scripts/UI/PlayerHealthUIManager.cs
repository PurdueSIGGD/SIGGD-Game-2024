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

    [SerializeField] private GameObject pipBackground1;
    [SerializeField] private GameObject pipBackground2;
    [SerializeField] private GameObject pipBackground3;

    [SerializeField] private Image background;
    [SerializeField] private Sprite healthyBackground;
    [SerializeField] private Sprite woundedBackground;
    [SerializeField] private Sprite mortallyWoundedBackground;
    [SerializeField] private Sprite deadBackground;

    [SerializeField] private Image foreground;
    [SerializeField] private Sprite healthyForeground;
    [SerializeField] private Sprite woundedForeground;
    [SerializeField] private Sprite mortallyWoundedForeground;
    [SerializeField] private Sprite deadForeground;

    private PlayerHealth health;
    private StatManager stats;

    private bool isWounded = false;
    private bool isMortallyWounded = false;
    private bool isDead = false;



    void Start()
    {
        health = PlayerID.instance.GetComponent<PlayerHealth>();
        stats = health.GetStats();

        healthSlider.value = Mathf.Lerp(0.311f, 1f, (health.currentHealth / stats.ComputeValue("Max Health")));

        if (health.Wounded)
        {
            pipBackground1.SetActive(false);
            background.sprite = woundedBackground;
            foreground.sprite = woundedForeground;
            isWounded = true;
            isMortallyWounded = false;
            isDead = false;
        }
        else if (health.MortallyWounded)
        {
            pipBackground1.SetActive(false);
            pipBackground2.SetActive(false);
            background.sprite = mortallyWoundedBackground;
            foreground.sprite = mortallyWoundedForeground;
            isWounded = false;
            isMortallyWounded = true;
            isDead = false;
        }
        /*
        else if (!health.isAlive)
        {
            pipBackground1.SetActive(false);
            pipBackground2.SetActive(false);
            pipBackground3.SetActive(false);
            background.sprite = deadBackground;
            foreground.sprite = deadForeground;
            isWounded = false;
            isMortallyWounded = false;
            isDead = true;
        }
        */
        else
        {
            background.sprite = healthyBackground;
            foreground.sprite = healthyForeground;
            isWounded = false;
            isMortallyWounded = false;
            isDead = false;
        }
    }
    
    void Update()
    {
        UpdateHealthWidget();
        UpdateMortalWoundPips();
    }



    private void UpdateHealthWidget()
    {
        healthText.text = Mathf.CeilToInt(health.currentHealth) + " | " + Mathf.CeilToInt(stats.ComputeValue("Max Health"));

        float woundHealthThreshold = stats.ComputeValue("Wounded Threshold") * stats.ComputeValue("Max Health");
        float mortalWoundHealthThreshold = stats.ComputeValue("Mortal Wound Threshold") * stats.ComputeValue("Max Health");
        float healthPipAmount = mortalWoundHealthThreshold;

        if (isWounded) healthSlider.value = Mathf.Lerp(0.52f, 0.775f, ((health.currentHealth - mortalWoundHealthThreshold) / (healthPipAmount)));
        else if (isMortallyWounded) healthSlider.value = Mathf.Lerp(0.311f, 0.52f, (health.currentHealth / (healthPipAmount)));
        else if (!health.isAlive) healthSlider.value = 0f;
        else healthSlider.value = Mathf.Lerp(0.775f, 1f, ((health.currentHealth - woundHealthThreshold) / (healthPipAmount)));
    }



    private void UpdateMortalWoundPips()
    {
        /*
        if (health.Wounded)
        {
            pipBackground1.SetActive(false);
            isWounded = true;
        }
        if (health.MortallyWounded)
        {
            pipBackground2.SetActive(false);
            isMortallyWounded = true;
        }
        if (health.currentHealth <= 0f)
        {
            pipBackground3.SetActive(false);
            isDead = true;
        }

        if (!health.isAlive)
        {
            if (!isDead) DeathReceived();
            isWounded = false;
            isMortallyWounded = false;
            isDead = true;
        }
        */

        // Dead Check
        if (!health.isAlive)
        {
            if (!isDead) DeathReceived();
            isWounded = false;
            isMortallyWounded = false;
            isDead = true;
        }

        // Mortal Wound Check
        else if (health.MortallyWounded)
        {
            if (!isMortallyWounded)
            {
                if (!isDead) MortalWoundReceived();
                if (isDead) DeathHealed();
            }
            isWounded = false;
            isMortallyWounded = true;
            isDead = false;
        }

        // Wound Check
        else if (health.Wounded)
        {
            if (!isWounded)
            {
                if (!isMortallyWounded) WoundReceived();
                if (isMortallyWounded) MortalWoundHealed();
            }
            isWounded = true;
            isMortallyWounded = false;
            isDead = false;
        }

        // Healthy Check
        else
        {
            if (isWounded) WoundHealed();
            isWounded = false;
            isMortallyWounded = false;
            isDead = false;
        }
    }



    private void MortalWoundReceived()
    {
        pipBackground2.SetActive(false);
        background.sprite = mortallyWoundedBackground;
        foreground.sprite = mortallyWoundedForeground;
    }

    private void MortalWoundHealed()
    {
        pipBackground2.SetActive(true);
        background.sprite = woundedBackground;
        foreground.sprite = woundedForeground;
    }

    private void WoundReceived()
    {
        pipBackground1.SetActive(false);
        background.sprite = woundedBackground;
        foreground.sprite = woundedForeground;
    }

    private void WoundHealed()
    {
        pipBackground1.SetActive(true);
        background.sprite = healthyBackground;
        foreground.sprite = healthyForeground;
    }

    private void DeathReceived()
    {
        pipBackground3.SetActive(false);
        background.sprite = deadBackground;
        foreground.sprite = deadForeground;
    }

    private void DeathHealed()
    {
        pipBackground3.SetActive(true);
        background.sprite = mortallyWoundedBackground;
        foreground.sprite = mortallyWoundedForeground;
    }

}
