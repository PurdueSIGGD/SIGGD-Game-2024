using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [HideInInspector] public static PlayerParticles instance;

    [SerializeField] ParticleSystem heavyCharging;
    [SerializeField] Color heavyChargingColor;

    [SerializeField] ParticleSystem heavyPrimed;
    [SerializeField] Color heavyPrimedColor;

    [SerializeField] ParticleSystem heavyAttack;
    [SerializeField] ParticleSystem lightAttack;
    [SerializeField] ParticleSystem upLightAttack;

    [SerializeField] ParticleSystem ghostBack;
    [SerializeField] ParticleSystem ghostFront;

    [SerializeField] ParticleSystem ghostEmpowered;
    [SerializeField] float minEmpoweredEmission;
    [SerializeField] float maxEmpoweredEmission;

    [SerializeField] ParticleSystem ghostGoodBuff;
    [SerializeField] float minGoodBuffEmission;
    [SerializeField] float maxGoodBuffEmission;

    [SerializeField] ParticleSystem ghostBadBuff;
    [SerializeField] float minBadBuffEmission;
    [SerializeField] float maxBadBuffEmission;

    [SerializeField] ParticleSystem radiantWellBuff;
    [SerializeField] Color radiantWellColor;

    [SerializeField] GameObject pulseVFX;

    //private Color heavyChargingColor;
    //private Color heavyChargingColor;
    private CharacterSO selectedGhostSO;



    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //baseHeavyChargingColor = heavyCharging.main.startColor.color;
    }



    // Ghost Swap
    public void PlayGhostSelected(CharacterSO ghostSO)
    {
        // Update selected Ghost
        selectedGhostSO = ghostSO;

        // Update heavy attack color
        ParticleSystem.MainModule heavyChargingMain = heavyCharging.main;
        if (ghostSO.displayName.Equals("North") ||
            ghostSO.displayName.Equals("Akihito") ||
            ghostSO.displayName.Equals("King Aegis"))
        {
            heavyChargingMain.startColor = new ParticleSystem.MinMaxGradient(ghostSO.primaryColor);
        }

        // Pulse VFX
        GameObject swapPulse = Instantiate(pulseVFX, transform.position, Quaternion.identity);
        swapPulse.GetComponent<RingExplosionHandler>().playRingExplosion(2f, ghostSO.primaryColor);

        // Particle VFX
        ghostBack.Play();
        ghostFront.Play();
        ParticleSystem.MainModule ghostBackMain = ghostBack.main;
        ghostBackMain.startColor = new ParticleSystem.MinMaxGradient(ghostSO.primaryColor, ghostSO.highlightColor);
        ParticleSystem.MainModule ghostFrontMain = ghostFront.main;
        ghostFrontMain.startColor = new ParticleSystem.MinMaxGradient(ghostSO.primaryColor, ghostSO.highlightColor);
    }

    public void PlayGhostDeselected(CharacterSO ghostSO)
    {
        selectedGhostSO = null;
        ParticleSystem.MainModule heavyChargingMain = heavyCharging.main;
        heavyChargingMain.startColor = new ParticleSystem.MinMaxGradient(heavyChargingColor);
        ghostBack.Stop();
        ghostFront.Stop();
    }



    // Ghost Empowered
    public void PlayGhostEmpowered(Color color, float currentValue, float maxValue)
    {
        float valueScale = currentValue / maxValue;
        ParticleSystem.EmissionModule emmisionModule = ghostEmpowered.emission;
        emmisionModule.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.Lerp(minEmpoweredEmission, maxEmpoweredEmission, valueScale));
        ParticleSystem.MainModule ghostEmpoweredMain = ghostEmpowered.main;
        ghostEmpoweredMain.startColor = new ParticleSystem.MinMaxGradient(color);
        if (!ghostEmpowered.isPlaying) ghostEmpowered.Play();
    }

    public void StopGhostEmpowered()
    {
        if (ghostEmpowered.isPlaying) ghostEmpowered.Stop();
    }



    // Ghost Good Buff
    public void PlayGhostGoodBuff(Color color, float currentValue, float maxValue)
    {
        float valueScale = currentValue / maxValue;
        ParticleSystem.EmissionModule emmisionModule = ghostGoodBuff.emission;
        emmisionModule.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.Lerp(minGoodBuffEmission, maxGoodBuffEmission, valueScale));
        ParticleSystem.MainModule ghostGoodBuffMain = ghostGoodBuff.main;
        ghostGoodBuffMain.startColor = new ParticleSystem.MinMaxGradient(color);
        if (!ghostGoodBuff.isPlaying) ghostGoodBuff.Play();
    }

    public void StopGhostGoodBuff()
    {
        if (ghostGoodBuff.isPlaying) ghostGoodBuff.Stop();
    }



    // Ghost Bad Buff
    public void PlayGhostBadBuff(Color color, float currentValue, float maxValue)
    {
        float valueScale = currentValue / maxValue;
        ParticleSystem.EmissionModule emmisionModule = ghostBadBuff.emission;
        emmisionModule.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.Lerp(minBadBuffEmission, maxBadBuffEmission, valueScale));
        ParticleSystem.MainModule ghostBadBuffMain = ghostBadBuff.main;
        ghostBadBuffMain.startColor = new ParticleSystem.MinMaxGradient(color);
        if (!ghostBadBuff.isPlaying) ghostBadBuff.Play();
    }

    public void StopGhostBadBuff()
    {
        if (ghostBadBuff.isPlaying) ghostBadBuff.Stop();
    }



    // Aegis Radiant Well
    public void PlayRadiantWellBuff()
    {
        GameObject swapPulse = Instantiate(pulseVFX, transform.position, Quaternion.identity);
        swapPulse.GetComponent<RingExplosionHandler>().playRingExplosion(2f, radiantWellColor);
        if (!radiantWellBuff.isPlaying)
        {
            radiantWellBuff.Play();
        }
    }

    public void StopRadiantWellBuff()
    {
        if (radiantWellBuff.isPlaying)
        {
            GameObject swapPulse = Instantiate(pulseVFX, transform.position, Quaternion.identity);
            swapPulse.GetComponent<RingExplosionHandler>().playRingExplosion(2f, radiantWellColor);
            radiantWellBuff.Stop();
        }
    }



    // Orion Dash
    public void PlayOrionDash(Color primaryColor, Color secondaryColor)
    {
        ParticleSystem.MainModule ghostFrontMain = ghostFront.main;
        ghostFrontMain.startColor = new ParticleSystem.MinMaxGradient(primaryColor, secondaryColor);
        ghostFront.Play();
    }

    public void StopOrionDash()
    {
        ghostFront.Stop();
    }



    // Orion Glide
    public void PlayOrionGlide(Color primaryColor, Color secondaryColor)
    {
        if (selectedGhostSO != null) return;
        ParticleSystem.MainModule ghostFrontMain = ghostFront.main;
        ghostFrontMain.startColor = new ParticleSystem.MinMaxGradient(primaryColor, secondaryColor);
        ghostFront.Play();
    }

    public void StopOrionGlide()
    {
        if (selectedGhostSO != null) return;
        ghostFront.Stop();
    }



    // Heavy Attack
    public void StartHeavyChargeUp()
    {
        if (selectedGhostSO != null && selectedGhostSO.displayName.Equals("King Aegis")) return;
        heavyCharging.Play();
    }

    public void StopHeavyChargeUp()
    {
        heavyCharging.Stop();
    }

    public void StartHeavyPrimed()
    {
        GameObject swapPulse = Instantiate(pulseVFX, transform.position, Quaternion.identity);
        swapPulse.GetComponent<RingExplosionHandler>().playRingExplosion(2f, heavyPrimedColor);
        heavyPrimed.Play();
    }

    public void StopHeavyPrimed()
    {
        heavyPrimed.Stop();
    }

    public void PlayHeavyAttackVFX()
    {
        StartCoroutine(HeavyAttackCoroutine());
    }

    private IEnumerator HeavyAttackCoroutine()
    {
        yield return new WaitForSeconds(0.12f);
        heavyAttack.Play();
    }



    // Light Attack
    public void PlayLightAttackVFX(bool isUp)
    {
        StartCoroutine(LightAttackCoroutine(isUp));
    }

    private IEnumerator LightAttackCoroutine(bool isUp)
    {
        yield return new WaitForSeconds(0.05f);
        if (isUp)
        {
            upLightAttack.Play();
            yield break;
        }
        lightAttack.Play();
    }



    // Police Chief Heavy Attack
    public void StartSidearmChargeUp()
    {
        heavyCharging.Play();
    }

    public void StopSidearmChargeUp()
    {
        heavyCharging.Stop();
    }

    public void StartSidearmPrimed()
    {
        GameObject swapPulse = Instantiate(pulseVFX, transform.position, Quaternion.identity);
        swapPulse.GetComponent<RingExplosionHandler>().playRingExplosion(2f, heavyPrimedColor);
        heavyPrimed.Play();
    }

    public void StopSidearmPrimed()
    {
        heavyPrimed.Stop();
    }



    // Police Chief Special Ability
    public void StartSpecialChargeUp()
    {
        heavyCharging.Play();
    }

    public void StopSpecialChargeUp()
    {
        heavyCharging.Stop();
    }

    public void StartSpecialPrimed()
    {
        GameObject swapPulse = Instantiate(pulseVFX, transform.position, Quaternion.identity);
        swapPulse.GetComponent<RingExplosionHandler>().playRingExplosion(2f, heavyPrimedColor);
        heavyPrimed.Play();
    }

    public void StopSpecialPrimed()
    {
        heavyPrimed.Stop();
    }
}
