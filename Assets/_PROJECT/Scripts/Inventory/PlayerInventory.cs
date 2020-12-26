using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CasterData
{
    // General
    public string ID;
    public int BaseRarity;
    public int MaxLevel;
    public int MaxHealth;
    public int MaxAttack;
    public int MaxDefence;
    public string CharacterDescription;
    
    // Skill
    public string SkillDescription;

    // UI/Visual
    public GameObject FullArtPrefab;
    public GameObject SpriteHolderPrefab;
    public Sprite PortraitArt;
}

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    
    public List<UnitObject> playerCastersInventory = new List<UnitObject>();
    [SerializeField] List<CasterData> playerCastersData = new List<CasterData>();

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
        
        GetData();
    }

    public void GetData()
    {
        for (int i = 0; i < playerCastersInventory.Count; i++)
        {
            CasterData casterUnit = new CasterData();

            casterUnit.ID = playerCastersInventory[i].ID;
            casterUnit.BaseRarity = playerCastersInventory[i].BaseRarity;
            casterUnit.MaxLevel = playerCastersInventory[i].MaxLevel;
            casterUnit.MaxHealth = playerCastersInventory[i].MaxHealth;
            casterUnit.MaxAttack = playerCastersInventory[i].MaxAttack;
            casterUnit.MaxDefence = playerCastersInventory[i].MaxDefence;
            casterUnit.CharacterDescription = playerCastersInventory[i].CharacterDescription;
            
            casterUnit.SkillDescription = playerCastersInventory[i].SkillDescription;
            
            casterUnit.FullArtPrefab = playerCastersInventory[i].FullArtPrefab;
            casterUnit.SpriteHolderPrefab = playerCastersInventory[i].SpriteHolderPrefab;
            casterUnit.PortraitArt = playerCastersInventory[i].PortraitArt;
            
            playerCastersData.Add(casterUnit);
        }
    }

    #region Accessors

    public List<CasterData> GetPlayerCastersData() => playerCastersData;

    #endregion

}
