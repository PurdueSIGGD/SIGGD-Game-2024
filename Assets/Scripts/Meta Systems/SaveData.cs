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

    public NorthData north = new();
    public EvaData eva = new();
    public YumeData yume = new();
    public AkihitoData akihito = new();
    public SilasData silas = new();
    public AegisData aegis = new();
}


[Serializable]
public class GhostData
{
    public int storyProgress = 0; // progress through the ghost's story
    public int bossProgress = 0; // if the ghost has encountered their boss before
    public int xp = 0;
}

[Serializable]
public class NorthData : GhostData
{
    public int reserveSpecialCharges;
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
public class AkihitoData : GhostData
{

}

[Serializable]
public class SilasData : GhostData
{

}

[Serializable]
public class AegisData : GhostData
{
    public float damageDealtTillSmite = 0;
    public float damageBlockTillSmite = 0;
}