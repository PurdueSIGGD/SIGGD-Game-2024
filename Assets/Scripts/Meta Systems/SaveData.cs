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


    public NorthData north;
    public EvaData eva;
    public YumeData yume;
    public AegisData aegis;
}


[Serializable]
public class GhostData
{
    public bool isUnlocked = false;
    public int storyProgress = 0;
    public int xp = 0;
}

[Serializable]
public class NorthData : GhostData
{

}

[Serializable]
public class EvaData : GhostData
{

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