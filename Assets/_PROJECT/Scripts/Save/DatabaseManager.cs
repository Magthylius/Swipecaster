using System;
using System.Collections;
using System.Collections.Generic;
using LerpFunctions;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;
    
    [SerializeField] PlayerInventoryData playerData;
    
    [SerializeField] List<string> liveCaster = new List<string>();

    PlayerInventory playerInventory;

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
        
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        playerInventory = PlayerInventory.instance;
        
        playerData = SaveManager.Load();

        foreach (var caster in playerData.casterDatabase)
        {
            if (caster.IsAlive)
            {
                liveCaster.Add(caster.ID);
            }
        }

        CheckCasterIsAlive();
        
        playerInventory.SetPlayerInventory(liveCaster);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveManager.Save(playerData);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveManager.Load();
        }
    }

    public void SetCasterToAlive(CasterDataStats _caster)
    {
        int CDIndex = playerData.casterDatabase.IndexOf(_caster);
        
        if (!playerData.casterDatabase[CDIndex].IsAlive)
        {
            playerData.casterDatabase[CDIndex].IsAlive = true;
            CheckCasterIsAlive();
            SaveManager.Save(playerData);
            
        }
        else
            print("Caster already alive");
    }

    public void SetCasterToDead(CasterDataStats _caster)
    {
        int CDIndex = playerData.casterDatabase.IndexOf(_caster);
        
        if (playerData.casterDatabase[CDIndex].IsAlive)
        {
            playerData.casterDatabase[CDIndex].IsAlive = false;
            CheckCasterIsAlive();
            SaveManager.Save(playerData);
        }
        else
            print("Caster already dead");
    }

    void CheckCasterIsAlive()
    {
        liveCaster = new List<string>();
        
        foreach (var caster in playerData.casterDatabase)
        {
            if (caster.IsAlive)
            {
                liveCaster.Add(caster.ID);
            }
        }
    }
    
    #region Accessors
    
    public List<PartyData> GetPartyData() => playerData.partyDatabase;
    public List<CasterDataStats> GetCasterDataStats() => playerData.casterDatabase;
    
    #endregion


}
