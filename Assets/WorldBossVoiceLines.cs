using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldBossVoiceLines : MonoBehaviour
{
    public static WorldBossVoiceLines Instance;

    public string activeSceneName;

    public bool isCyberpunk = false;
    public bool isJapan = false;
    public bool isMedieval = false;
    public bool isStinky = false;

    public bool isCyberpunkBoss = false;
    public bool isJapanBoss = false;
    public bool isMedievalBoss = false;

    public string worldBossName;


    private void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        activeSceneName = SceneManager.GetActiveScene().name;
        string doorName = "Stinky";
        if (activeSceneName.Contains("_"))
        {
            doorName = activeSceneName.Substring(0, activeSceneName.IndexOf("_"));
        }
        if (doorName.Equals("Cyberpunk"))
        {
            isCyberpunk = true;
            worldBossName = "IRIS";
        }
        else if (doorName.Equals("Japan"))
        {
            isJapan = true;
            worldBossName = "Noboru";
        }
        else if (doorName.Equals("Medieval"))
        {
            isMedieval = true;
            worldBossName = "Scathe";
        }
        else
        {
            isStinky = true;
        }

        if (activeSceneName.Equals("Cyberpunk_Boss"))
        {
            isCyberpunkBoss = true;
        }
        else if (activeSceneName.Equals("Japan_Boss"))
        {
            isCyberpunkBoss = true;
        }
        else if (activeSceneName.Equals("Medieval_Boss"))
        {
            isCyberpunkBoss = true;
        }

        StartCoroutine(OnEnterCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private IEnumerator OnEnterCoroutine()
    {
        if (isCyberpunkBoss || isJapanBoss || isMedievalBoss) yield break;
        if (isStinky) yield break;
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.VABranch.PlayVATrack(worldBossName + " On Player Enter");
    }





    public void PlayOnNewWave()
    {
        StartCoroutine(NewWaveCoroutine());
    }

    private IEnumerator NewWaveCoroutine()
    {
        if (isStinky) yield break;
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.VABranch.PlayVATrack(worldBossName + " On New Wave");
    }





    public void PlayOnClearRoom()
    {
        if (isCyberpunkBoss || isJapanBoss || isMedievalBoss) return;
        StartCoroutine(ClearRoomCoroutine());
    }

    private IEnumerator ClearRoomCoroutine()
    {
        if (isStinky) yield break;
        yield return new WaitForSeconds(0.9f);
        AudioManager.Instance.VABranch.PlayVATrack(worldBossName + " On Clear Room");
    }


}
