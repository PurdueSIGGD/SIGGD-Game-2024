using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using UnityEngine.SceneManagement;

public class AchievementTracker : MonoBehaviour
{
    public static AchievementTracker instance;

    [SerializeField] private bool resetStatsInHub = false;
    [SerializeField] private bool alsoResetAchievementsInHub = false;


    public string killsStatName = "kills";
    public string kills1Name = "KILLS_1";
    public string kills2Name = "KILLS_2";
    public string kills3Name = "KILLS_3";
    public string kills4Name = "KILLS_4";
    public string kills5Name = "KILLS_5";

    public string irisName = "IRIS_KILL";
    public string noboruName = "NOBORU_KILL";
    public string scatheName = "SCATHE_KILL";
    public string oldrionName = "OLDRION_KILL";

    public string mastery1Name = "MASTERY_1";
    public string masteryAllName = "MASTERY_ALL";
    public string allGhostsMaxName = "ALL_GHOSTS_MAX";

    public string northNewName = "NORTH_NEW";
    public string northSacName = "NORTH_SAC";
    public string northMaxName = "NORTH_MAX";

    public string evaNewName = "EVA_NEW";
    public string evaSacName = "EVA_SAC";
    public string evaMaxName = "EVA_MAX";





    private void OnEnable()
    {
        GameplayEventHolder.OnDeath += OnPlayerKill;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDeath -= OnPlayerKill;
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!SteamManager.Initialized) return;
        if (resetStatsInHub && SceneManager.GetActiveScene().name.Equals("HubWorld"))
        {
            SteamUserStats.ResetAllStats(alsoResetAchievementsInHub);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnPlayerKill(DamageContext context)
    {
        if (!context.victim.CompareTag("Enemy") || context.attacker.CompareTag("Enemy")) return;

        if (!SteamManager.Initialized)
        {
            Debug.Log("STEAM Kills: SteamManager not initialized!");
            return;
        }

        SteamUserStats.GetStat(killsStatName, out int killsStat);
        Debug.Log("STEAM Kills: current killsStat: " + killsStat);
        killsStat++;
        SteamUserStats.SetStat(killsStatName, killsStat);
        SteamUserStats.StoreStats();
        Debug.Log("STEAM Kills: new killsStat: " + killsStat);

        SteamUserStats.GetStat(killsStatName, out int killsStatUpdated);
        Debug.Log("STEAM Kills: new steam killsStat: " + killsStatUpdated);

        /*
        SteamUserStats.GetAchievement(kills1Name, out bool kills1Complete);
        SteamUserStats.GetAchievementProgressLimits(kills1Name, out int minKills, out int maxKills);
        if (!kills1Complete && killsStat >= maxKills)
        {
            SteamUserStats.SetAchievement(kills1Name);
            SteamUserStats.StoreStats();
        }
        */
    }



    public void SetAchievement(string achievementName)
    {
        if (!SteamManager.Initialized)
        {
            Debug.Log("STEAM " + achievementName + ": SteamManager not initialized!");
            return;
        }

        SteamUserStats.GetAchievement(achievementName, out bool achievementComplete);
        Debug.Log("STEAM " + achievementName + ": current achieved: " + achievementComplete);
        if (!achievementComplete)
        {
            SteamUserStats.SetAchievement(achievementName);
            SteamUserStats.StoreStats();
            Debug.Log("STEAM " + achievementName + ": new achieved: true");

            SteamUserStats.GetAchievement(achievementName, out bool achievementUpdated);
            Debug.Log("STEAM " + achievementName + ": new steam achieved: " + achievementUpdated);
        }
    }



    public void SetGhostAchievement(string ghostName, GhostAchievement achievementType)
    {
        string achievementGhostPrefix = ghostName switch
        {
            "North" => "NORTH",
            "Eva" => "EVA",
            "Akihito" => "AKIHITO",
            "Yume" => "YUME",
            "Silas" => "SILAS",
            "King Aegis" => "AEGIS",
            _ => "NORTH",
        };

        string achievementTypeSuffix = achievementType switch
        {
            GhostAchievement.NEW => "_NEW",
            GhostAchievement.SACRIFICE => "_SAC",
            GhostAchievement.MAX => "_MAX",
            _ => "_NEW",
        };

        string achievementName = achievementGhostPrefix + achievementTypeSuffix;
        SetAchievement(achievementName);
    }
}



public enum GhostAchievement
{
    NEW,
    SACRIFICE,
    MAX,
}