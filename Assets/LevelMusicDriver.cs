using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicDriver : MonoBehaviour
{
    [SerializeField] private MusicTrackName roomMusicTrack;
    [SerializeField] private float baselineEnergyCrossfadeTime;
    [SerializeField] private float zeroEnergyCrossfadeTime;
    [SerializeField] private float baselineEnergy;

    [SerializeField] private float healthEnergyThreshold;
    [SerializeField] private float energyPerHealthLost;

    [SerializeField] private float OnKillHighSpeedCombatEnergyGain;

    [SerializeField] private float enemyCountEnergyThreshold;
    [SerializeField] private float energyPerEnemy;

    [SerializeField] private float highEnergyDecayRate;

    private AudioManager audioManager;
    public EnemySpawning enemySpawning;
    private Health playerHealth;

    private bool isCrossfading = false;
    private float highEnergyModifier = 0f;



    void OnEnable()
    {
        GameplayEventHolder.OnDeath += OnKillHighSpeedCombatModifier;
    }
    void OnDisable()
    {
        GameplayEventHolder.OnDeath -= OnKillHighSpeedCombatModifier;
    }



    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.Instance;
        if (GameObject.FindGameObjectWithTag("Player") != null) playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        if (audioManager == null) return;
        if (audioManager.MusicBranch.GetCurrentMusicTrackName() == roomMusicTrack)
        {
            isCrossfading = true;
            StartCoroutine(CrossfadeToBaseLevel());
            return;
        }
        if (audioManager.MusicBranch.GetCurrentMusicTrackName() == MusicTrackName.NULL)
        {
            audioManager.MusicBranch.PlayMusicTrack(roomMusicTrack);
            return;
        }
        audioManager.MusicBranch.CrossfadeTo(roomMusicTrack, 3f);
    }



    // Update is called once per frame
    void Update()
    {
        if (enemySpawning == null || audioManager == null || isCrossfading) return;

        // Crossfade to level zero at the end of combat
        if (enemySpawning.GetCurrentEnemies().Count <= 0 && audioManager.GetEnergyLevel() > 0f && !isCrossfading)
        {
            isCrossfading = true;
            StartCoroutine(CrossfadeToLevelZero());
            return;
        }
        if (enemySpawning.GetCurrentEnemies().Count <= 0) return;



        float thisEnergyLevel = baselineEnergy;

        // Low player health energy gain
        if (playerHealth != null) thisEnergyLevel += Mathf.Max((healthEnergyThreshold - playerHealth.currentHealth) * energyPerHealthLost, 0f); // Increase intensity as player health decreases

        // Enemy count energy
        thisEnergyLevel += Mathf.Max((enemySpawning.GetCurrentEnemies().Count - enemyCountEnergyThreshold) * energyPerEnemy, 0f);

        // High energy decay
        highEnergyModifier = Mathf.Max(highEnergyModifier - (highEnergyDecayRate * Time.deltaTime), 0f);
        thisEnergyLevel += highEnergyModifier;

        audioManager.SetEnergyLevel(Mathf.Clamp(thisEnergyLevel, baselineEnergy, 1f));
    }



    private void OnKillHighSpeedCombatModifier(DamageContext damage)
    {
        highEnergyModifier = Mathf.Clamp(highEnergyModifier + OnKillHighSpeedCombatEnergyGain, 0f, 1f - baselineEnergy);
    }



    private IEnumerator CrossfadeToBaseLevel()
    {
        isCrossfading = true;
        audioManager.SetEnergyLevel(0f);
        int step = 20;
        for (int i = 0; i < step; i++)
        {
            if (enemySpawning != null && enemySpawning.GetCurrentEnemies().Count <= 0)
            {
                isCrossfading = false;
                yield break;
            }
            audioManager.SetEnergyLevel(((float) i / (float) step) * baselineEnergy);
            yield return new WaitForSeconds(baselineEnergyCrossfadeTime / (float) step);
        }
        audioManager.SetEnergyLevel(baselineEnergy);
        isCrossfading = false;
    }



    private IEnumerator CrossfadeToLevelZero()
    {
        isCrossfading = true;
        float currentEnergy = audioManager.GetEnergyLevel();
        audioManager.SetEnergyLevel(currentEnergy);
        int step = 20;
        for (int i = 0; i < step; i++)
        {
            if (enemySpawning.GetCurrentEnemies().Count > 0)
            {
                isCrossfading = false;
                yield break;
            }
            audioManager.SetEnergyLevel(currentEnergy - (((float) i / (float) step) * currentEnergy));
            yield return new WaitForSeconds(zeroEnergyCrossfadeTime / (float) step);
        }
        audioManager.SetEnergyLevel(0f);
        isCrossfading = false;
    }
}
