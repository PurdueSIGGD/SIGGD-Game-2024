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

    [SerializeField] private List<Sprite> woundBackgroundAnimationFrames;
    [SerializeField] private List<Sprite> mortalWoundBackgroundAnimationFrames;
    [SerializeField] private List<Sprite> deathBackgroundAnimationFrames;

    [SerializeField] private List<Sprite> woundForegroundAnimationFrames;
    [SerializeField] private List<Sprite> mortalWoundForegroundAnimationFrames;
    [SerializeField] private List<Sprite> deathForegroundAnimationFrames;

    [SerializeField] private float animationFrameDelay;

    private List<Sprite> backgroundAnimationFrameQueue;
    private List<Sprite> foregroundAnimationFrameQueue;
    private bool isAnimating = false;
    private float animationTimer = 0f;

    private PlayerHealth health;
    private StatManager stats;

    private bool isWounded = false;
    private bool isMortallyWounded = false;
    private bool isDead = false;

    private bool hasStarted = false;
    private float startCounter = 5f;



    void Start()
    {
        startCounter = 5f;
        backgroundAnimationFrameQueue = new List<Sprite>();
        foregroundAnimationFrameQueue = new List<Sprite>();
        health = PlayerID.instance.GetComponent<PlayerHealth>();
        stats = health.GetStats();
    }
    
    void Update()
    {
        UpdateMortalWoundPips();
        UpdateHealthWidget();
        UpdateAnimator();
        if (startCounter <= 0f && !hasStarted) hasStarted = true;
    }



    private void UpdateAnimator()
    {
        if (isAnimating && animationTimer > 0f)
        {
            animationTimer -= Time.deltaTime;
            if (animationTimer <= 0f)
            {
                animationTimer = animationFrameDelay;
                ShowNextAnimationFrame();
            }
        }
    }



    private void PlayAnimation(List<Sprite> backgroundAnimationFrames, List<Sprite> foregroundAnimationFrames)
    {
        backgroundAnimationFrameQueue.Clear();
        foregroundAnimationFrameQueue.Clear();
        backgroundAnimationFrameQueue.AddRange(backgroundAnimationFrames);
        foregroundAnimationFrameQueue.AddRange(foregroundAnimationFrames);
        ShowNextAnimationFrame();
        animationTimer = animationFrameDelay;
        isAnimating = true;
    }



    private void ShowNextAnimationFrame()
    {
        if (backgroundAnimationFrameQueue.Count <= 0f)
        {
            backgroundAnimationFrameQueue.Clear();
            foregroundAnimationFrameQueue.Clear();
            isAnimating = false;
            animationTimer = 0f;
            return;
        }
        background.sprite = backgroundAnimationFrameQueue[0];
        backgroundAnimationFrameQueue.RemoveAt(0);
        foreground.sprite = foregroundAnimationFrameQueue[0];
        foregroundAnimationFrameQueue.RemoveAt(0);
    }



    private List<Sprite> GetReversedAnimationFramesList(List<Sprite> animationFrames)
    {
        List<Sprite> reversedList = new List<Sprite>();
        for (int i = animationFrames.Count - 1; i >= 0; i--)
        {
            reversedList.Add(animationFrames[i]);
        }
        return reversedList;
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
            if (!hasStarted)
            {
                StartMortallyWounded();
                return;
            }
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
            if (!hasStarted)
            {
                StartWounded();
                return;
            }
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
            if (!hasStarted)
            {
                StartHealthy();
                return;
            }
            if (isWounded) WoundHealed();
            isWounded = false;
            isMortallyWounded = false;
            isDead = false;
        }
    }





    private void StartHealthy()
    {
        background.sprite = healthyBackground;
        foreground.sprite = healthyForeground;
        isWounded = false;
        isMortallyWounded = false;
        isDead = false;
        startCounter--;
        //hasStarted = true;
    }

    private void StartWounded()
    {
        pipBackground1.SetActive(false);
        background.sprite = woundedBackground;
        foreground.sprite = woundedForeground;
        isWounded = true;
        isMortallyWounded = false;
        isDead = false;
        startCounter--;
        //hasStarted = true;
    }

    private void StartMortallyWounded()
    {
        pipBackground1.SetActive(false);
        pipBackground2.SetActive(false);
        background.sprite = mortallyWoundedBackground;
        foreground.sprite = mortallyWoundedForeground;
        isWounded = false;
        isMortallyWounded = true;
        isDead = false;
        startCounter--;
        //hasStarted = true;
    }





    private void MortalWoundReceived()
    {
        pipBackground1.SetActive(false);
        pipBackground2.SetActive(false);
        PlayAnimation(mortalWoundBackgroundAnimationFrames, mortalWoundForegroundAnimationFrames);
        //background.sprite = mortallyWoundedBackground;
        //foreground.sprite = mortallyWoundedForeground;
    }

    private void MortalWoundHealed()
    {
        pipBackground2.SetActive(true);
        List<Sprite> backgroundAnimationFrames = GetReversedAnimationFramesList(mortalWoundBackgroundAnimationFrames);
        List<Sprite> foregroundAnimationFrames = GetReversedAnimationFramesList(mortalWoundForegroundAnimationFrames);
        PlayAnimation(backgroundAnimationFrames, foregroundAnimationFrames);
        //background.sprite = woundedBackground;
        //foreground.sprite = woundedForeground;
    }

    private void WoundReceived()
    {
        pipBackground1.SetActive(false);
        PlayAnimation(woundBackgroundAnimationFrames, woundForegroundAnimationFrames);
        //background.sprite = woundedBackground;
        //foreground.sprite = woundedForeground;
    }

    private void WoundHealed()
    {
        pipBackground1.SetActive(true);
        List<Sprite> backgroundAnimationFrames = GetReversedAnimationFramesList(woundBackgroundAnimationFrames);
        List<Sprite> foregroundAnimationFrames = GetReversedAnimationFramesList(woundForegroundAnimationFrames);
        PlayAnimation(backgroundAnimationFrames, foregroundAnimationFrames);
        //background.sprite = healthyBackground;
        //foreground.sprite = healthyForeground;
    }

    private void DeathReceived()
    {
        pipBackground1.SetActive(false);
        pipBackground2.SetActive(false);
        pipBackground3.SetActive(false);
        PlayAnimation(deathBackgroundAnimationFrames, deathForegroundAnimationFrames);
        //background.sprite = deadBackground;
        //foreground.sprite = deadForeground;
    }

    private void DeathHealed()
    {
        pipBackground3.SetActive(true);
        List<Sprite> backgroundAnimationFrames = GetReversedAnimationFramesList(deathBackgroundAnimationFrames);
        List<Sprite> foregroundAnimationFrames = GetReversedAnimationFramesList(deathForegroundAnimationFrames);
        PlayAnimation(backgroundAnimationFrames, foregroundAnimationFrames);
        //background.sprite = mortallyWoundedBackground;
        //foreground.sprite = mortallyWoundedForeground;
    }

}
