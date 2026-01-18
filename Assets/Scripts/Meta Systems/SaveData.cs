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

    public List<GhostToGhostProgressSaveData> ghostToGhostProgress = new()
    {
        new("north"), // north
        new("eva"), // eva
        new("akihito"), // akihito
        new("yume"), // yume
        new("silas"), // silas
        new("aegis")  // aegis
    };
}


[Serializable]
public abstract class GhostData
{
    public abstract int Index { get; }
    public int storyProgress = 0; // progress through the ghost's story
    public int bossProgress = 0; // if the ghost has encountered their boss before
    public int xp = 0;
    public float specialEnergy = 0f;
}

[Serializable]
public class NorthData : GhostData
{
    public override int Index => 0;
    public int reserveSpecialCharges;
    public int lethalForceProgress;
}

[Serializable]
public class EvaData : GhostData
{
    public override int Index => 1;
    public int tempoCount;
    public float remainingTempoDuration;
}

[Serializable]
public class AkihitoData : GhostData
{
    public override int Index => 2;
}

[Serializable]
public class YumeData : GhostData
{
    public override int Index => 3;
    public int spoolCount;
    public int scrapSaverCount;
}

[Serializable]
public class SilasData : GhostData
{
    public override int Index => 4;
    public int ingredientsCollected;
    public int specialCharges;
}

[Serializable]
public class AegisData : GhostData
{
    public override int Index => 5;
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

[Serializable]
public class GhostToGhostProgressSaveData // new class to bypass serialization limit
{
    public string name;
    public int[] row;

    public GhostToGhostProgressSaveData(string name)
    {
        row = new int[6];
        this.name = name;
    }
}