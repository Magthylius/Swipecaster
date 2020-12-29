using System.Collections.Generic;

[System.Serializable]
public class PlayerInventoryData
{

	public List<CasterDataStats> casterDatabase = new List<CasterDataStats>();
    public List<PartyData> partyDatabase = new List<PartyData>();

}

[System.Serializable]
public class CasterDataStats
{
    public string ID;
    public int CurLevel;
    public bool IsAlive;
	
	public CasterDataStats(string id, int level, bool isAlive)
	{
		ID = id;
		CurLevel = level;
        IsAlive = isAlive;
	}
}

[System.Serializable]
public class PartyData
{
    public List<int> casterConfig = new List<int>();
}