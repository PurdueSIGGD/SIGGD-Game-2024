using System.Collections.Generic;
using UnityEngine;

public class GhostExp : MonoBehaviour
{
    [SerializeField] List<int> levelReqs = new List<int>()
    { 2000, 2000, 2000, 
      2000, 2000, 2000, 
      4000, 4000, 4000, 
      5000, 5000, 5000, 
      5000, 5000, 5000 };

    private int curLevel;
    private int curExp;

    void Start()
    {
        string identityName = gameObject.name;

        if (identityName.Contains("(Clone)"))
        {
            identityName.Replace("(Clone)", "");
        }

        curLevel = SaveManager.data.ghostLevel[identityName];

        switch (identityName)
        {
            case "Eva-Idol":
                curExp = SaveManager.data.eva.xp;
                break;
            case "North-Police_Chief":
                curExp = SaveManager.data.north.xp;
                break;
            case "Akihito-Samurai":
                curExp = SaveManager.data.akihito.xp;
                break;
            case "Yume-Seamstress":
                curExp = SaveManager.data.yume.xp;
                break;
            case "Silas-PlagueDoc":
                curExp = SaveManager.data.silas.xp;
                break;
            case "Aegis-King":
                curExp = SaveManager.data.aegis.xp;
                break;
            default:
                Debug.LogError("Cannot parse ghost's name attmpting to level up: " + identityName);
                return;
        }
        AttemptLevelUp();
    }


    void OnEnable()
    {
        Spirit.SpiritCollected += GainExp;
    }

    void OnDisable()
    {
        Spirit.SpiritCollected -= GainExp;   
    }

    private void GainExp(Spirit.SpiritType type)
    {
        
    }

    private void AttemptLevelUp()
    {

    }
}
