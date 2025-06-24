using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<string> ghostsInParty = new();
    public string selectedGhost = "Orion";
}