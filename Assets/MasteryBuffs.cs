using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasteryBuffs : MonoBehaviour
{
    private StatManager stats;

    [SerializeField] private float hasteLevelBuff;
    [SerializeField] private float lethalityLevelBuff;
    [SerializeField] private float fortitudeLevelBuff;

    [SerializeField] private float dexterityLevelBuff;
    [SerializeField] private float precisionLevelBuff;
    [SerializeField] private float tenacityLevelBuff;

    [SerializeField] private float proficiencyLevelBuff;
    [SerializeField] private float brutalityLevelBuff;
    [SerializeField] private float recoveryLevelBuff;



    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<StatManager>();

        StartCoroutine(LateStartCoroutine());
    }

    private IEnumerator LateStartCoroutine()
    {
        yield return new WaitForSeconds(0f);

        // Haste
        stats.ModifyStat("Max Running Speed", Mathf.FloorToInt(hasteLevelBuff * SaveManager.data.masteryUpgrades.upgradeLevels[0]));
        stats.ModifyStat("Running Accel.", Mathf.FloorToInt(hasteLevelBuff * SaveManager.data.masteryUpgrades.upgradeLevels[0]));
        stats.ModifyStat("Max Glide Speed", Mathf.FloorToInt(hasteLevelBuff * SaveManager.data.masteryUpgrades.upgradeLevels[0]));
        stats.ModifyStat("Glide Accel.", Mathf.FloorToInt(hasteLevelBuff * SaveManager.data.masteryUpgrades.upgradeLevels[0]));

        // Lethality
        stats.ModifyStat("General Attack Damage Boost", Mathf.FloorToInt(lethalityLevelBuff * SaveManager.data.masteryUpgrades.upgradeLevels[1]));

        // Fortitude
        Health playerHealth = GetComponent<Health>();
        stats.ModifyStat("Max Health", Mathf.FloorToInt(fortitudeLevelBuff * SaveManager.data.masteryUpgrades.upgradeLevels[2]));
        //float addedHealth = Mathf.Clamp(Mathf.Floor(fortitudeLevelBuff * SaveManager.data.masteryUpgrades.upgradeLevels[2]), 0f, stats.ComputeValue("Max Health") - playerHealth.currentHealth);
        //playerHealth.currentHealth += addedHealth;
        if (SceneManager.GetActiveScene().name.Equals("HubWorld")) playerHealth.currentHealth = stats.ComputeValue("Max Health");
        //GetComponent<Health>().currentHealth += Mathf.Floor(fortitudeLevelBuff * SaveManager.data.masteryUpgrades.upgradeLevels[2]);
        //GetComponent<Health>().Heal(Mathf.Floor(fortitudeLevelBuff * SaveManager.data.masteryUpgrades.upgradeLevels[2]), gameObject);


        // Dexterity
        stats.ModifyStat("Dodge Chance", Mathf.FloorToInt(dexterityLevelBuff * SaveManager.data.masteryUpgrades.upgradeLevels[3]) * 10);

        // Precision
        stats.ModifyStat("Crit Chance", Mathf.FloorToInt(precisionLevelBuff * SaveManager.data.masteryUpgrades.upgradeLevels[4]));

        // Tenacity
        stats.ModifyStat("Elite Damage Resistance", Mathf.FloorToInt(tenacityLevelBuff * SaveManager.data.masteryUpgrades.upgradeLevels[5]));


        // Proficiency
        stats.ModifyStat("Cooldown Speed Boost", Mathf.FloorToInt(proficiencyLevelBuff * SaveManager.data.masteryUpgrades.upgradeLevels[6]));

        // Brutality
        stats.ModifyStat("Stun Duration Boost", Mathf.FloorToInt(brutalityLevelBuff * SaveManager.data.masteryUpgrades.upgradeLevels[7]));

        // Recovery
        stats.ModifyStat("Healing Boost", Mathf.FloorToInt(recoveryLevelBuff * SaveManager.data.masteryUpgrades.upgradeLevels[8]));
    }



    // Update is called once per frame
    void Update()
    {
        
    }

}
