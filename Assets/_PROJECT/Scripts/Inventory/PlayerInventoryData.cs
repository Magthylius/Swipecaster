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

}

[System.Serializable]
public class CasterDataStats
{
    public string ID;
    public int BaseRarity;
    public int MaxLevel;
    public int MaxHealth;
    public int MaxAttack;
    public int MaxDefence;
    public string CharacterDescription;
}

[System.Serializable]
public class PartyData
{
    
}