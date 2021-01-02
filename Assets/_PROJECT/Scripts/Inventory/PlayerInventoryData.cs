using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInventoryData
{
	public List<CasterDataStats> casterDatabase = new List<CasterDataStats>();
    public List<PartyData> partyDatabase = new List<PartyData>();

    public Vector3 arrowTransform;
}

[System.Serializable]
public class CasterDataStats
{
    public string ID;
    public int CurLevel;
    public int Mastery;
    public bool IsAlive;
	
	public CasterDataStats(string id, int level, int mastery, bool isAlive)
	{
		ID = id;
		CurLevel = level;
		Mastery = mastery;
        IsAlive = isAlive;
	}
}

[System.Serializable]
public class PartyData
{
	public string PartyName;
    public List<string> CasterConfig;

    public PartyData(string partyName, List<string> casterConfig)
    {
	    PartyName = partyName;
	    CasterConfig = casterConfig;
    }
    
}