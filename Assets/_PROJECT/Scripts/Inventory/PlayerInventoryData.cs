using System.Collections.Generic;

[System.Serializable]
public class PlayerInventoryData
{

    public List<string> inventoryCasterDataSave = new List<string>()
    {
        //! player default caster
        "001",
        "002",
        "003",
        "004"
    };
    
    public List<CasterDataStats> casterDatabase = new List<CasterDataStats>();
    public List<PartyData> partyDatabase = new List<PartyData>();

}

[System.Serializable]
public class CasterDataStats
{
    public string ID;
    public int CurLevel;
	
	public CasterDataStats(string id, int level)
	{
		ID = id;
		CurLevel = level;
	}
}

[System.Serializable]
public class PartyData
{
    public List<int> casterConfig = new List<int>();
}