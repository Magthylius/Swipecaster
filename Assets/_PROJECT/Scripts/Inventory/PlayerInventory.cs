using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    public string casterLocation = "ScriptableObjects/Casters";

    [SerializeField] List<CasterData> playerCasterInventory = new List<CasterData>();
    
    //! Get all casters in the game
     readonly List<UnitObject> allCasters = new List<UnitObject>();
    [SerializeField] List<CasterData> playerCastersData = new List<CasterData>();

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;

    
        UnitObject[] tempCaster = Resources.LoadAll<UnitObject>(casterLocation);

        foreach (var _caster in tempCaster)
        {
            allCasters.Add(_caster);
        }
        
        CaptureData();
    }

    void CaptureData()
    {
        for (int i = 0; i < allCasters.Count; i++)
        {
            CasterData casterUnit = new CasterData();

            casterUnit.ID = allCasters[i].ID;
            casterUnit.BaseRarity = allCasters[i].BaseRarity;
            casterUnit.MaxLevel = allCasters[i].MaxLevel;
            casterUnit.MaxHealth = allCasters[i].MaxHealth;
            casterUnit.MaxAttack = allCasters[i].MaxAttack;
            casterUnit.MaxDefence = allCasters[i].MaxDefence;
            casterUnit.CharacterDescription = allCasters[i].CharacterDescription;
            
            casterUnit.SkillDescription = allCasters[i].SkillDescription;
            
            casterUnit.FullArtPrefab = allCasters[i].FullArtPrefab;
            casterUnit.SpriteHolderPrefab = allCasters[i].SpriteHolderPrefab;
            casterUnit.PortraitArt = allCasters[i].PortraitArt;
            
            playerCastersData.Add(casterUnit);
        }
    }

    public void SetPlayerInventory(List<string> castersID)
    {
        foreach (var id in castersID)
        {
            for (int i = 0; i < playerCastersData.Count; i++)
            {
                if (id == playerCastersData[i].ID)
                {
                    playerCasterInventory.Add(playerCastersData[i]);
                    break;
                }
            }
        }
    }

    #region Accessors

    public List<CasterData> GetPlayerCastersInventory() => playerCasterInventory;
    public List<UnitObject> GetAllCaster() => allCasters;

    #endregion
}

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
