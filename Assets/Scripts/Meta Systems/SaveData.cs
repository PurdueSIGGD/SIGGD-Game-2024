using JetBrains.Annotations;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<string> ghostsInParty = new();
    public string selectedGhost = "Orion";
    public Dictionary<string, int> ghostLevel = new();
    public Dictionary<string, int[]> ghostSkillPts = new();
    public List<string> saveGhostNames = new();
    public List<List<int>> saveGhostData = new();

    public int[] northSkillPts = new int[8];
    public int[] evaSkillPts = new int[8];
    public int[] akihitoSkillPts = new int[8];
    public int[] yumeSkillPts = new int[8];
    public int[] silasSkillPts = new int[8];
    public int[] aegisSkillPts = new int[8];

    public int death = 0;
    public int orion = 0; // story progress for Orion/Death
    public NorthData north = new();
    public EvaData eva = new();
    public YumeData yume = new();
    public AkihitoData akihito = new();
    public SilasData silas = new();
    public AegisData aegis = new();

    // 0: blue, 1: red, 2: yellow, 3: pink
    public int[] spiritCounts = new int[4];

    public MasteryUpgradeData masteryUpgrades = new();

    //public List<GhostToGhostProgressSaveData> ghostToGhostProgress = new() 
    //    { new("eva_north"), new("eva_akihito"), new("eva_yume"), new("eva_silas"), new("eva_aegis"), 
    //      new("north_akihito"), new("north_yume"), new("north_silas"), new("north_aegis"),
    //      new("akihito_yume"), new("akihito_silas"), new("akihito_aegis"),
    //      new("yume_silas"), new("yume_aegis"), new("silas_aegis")
    //    };
    public int[][] GhostToGhostProgress = new int[6][]
    {
        new int[6], // north
        new int[6], // eva
        new int[6], // akihito
        new int[6], // yume
        new int[6], // silas
        new int[6]  // aegis
    };
}


[Serializable]
public class GhostData
{
    public const int INDEX = 0;
    public int storyProgress = 0; // progress through the ghost's story
    public int bossProgress = 0; // if the ghost has encountered their boss before
    public int xp = 0;
}

[Serializable]
public class NorthData : GhostData
{
    public new const int INDEX = 0;
    public int reserveSpecialCharges;
}

[Serializable]
public class EvaData : GhostData
{
    public new const int INDEX = 1;
    public int tempoCount;
    public float remainingTempoDuration;
}

[Serializable]
public class AkihitoData : GhostData
{
    public new const int INDEX = 2;
}

[Serializable]
public class YumeData : GhostData
{
    public new const int INDEX = 3;
    public int spoolCount;
    public int scrapSaverCount;
}

[Serializable]
public class SilasData : GhostData
{
    public new const int INDEX = 4;
    public int ingredientsCollected;
}

[Serializable]
public class AegisData : GhostData
{
    public new const int INDEX = 5;
    public float damageDealtTillSmite = 0;
    public float damageBlockTillSmite = 0;
}

[Serializable]
public class MasteryUpgradeData
{
    public int numRowsUnlocked = 1;
    public int[] upgradeLevels = {
      // BLUE  RED   YELLOW
         0,    0,    0,      // Tier 1
         0,    0,    0,      // Tier 2
         0,    0,    0       // Tier 3
    };
}