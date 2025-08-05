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

    public EvaData eva = new();
    public YumeData yume = new();
    public AegisData aegis = new();

    // 0: red, 1: blue, 2: yellow
    public int[] spiritCounts = new int[3];
}


[Serializable]
public class GhostData
{
    public bool isUnlocked = false;
    public int storyProgress = 0;
    public int xp = 0;
}

[Serializable]
public class EvaData : GhostData
{
    public int tempoCount;
    public float remainingTempoDuration;
}

[Serializable]
public class YumeData : GhostData
{
    public int spoolCount;
}

[Serializable]
public class AegisData : GhostData
{
    public float damageDealtTillSmite = 0;
    public float damageBlockTillSmite = 0;
}