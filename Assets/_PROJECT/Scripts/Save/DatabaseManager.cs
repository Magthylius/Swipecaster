using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        LoadData();

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

    void LoadData()
    {
        playerData = SaveManager.Load();

        //! Check if the save file didn't exist and make a new save with default character
        if (playerData == null)
        {
            playerData = new PlayerInventoryData();
            playerData.casterDatabase.Add(new CasterDataStats("001", 1, true));
            playerData.casterDatabase.Add(new CasterDataStats("002", 1, true));
            playerData.casterDatabase.Add(new CasterDataStats("003", 1, true));
            playerData.casterDatabase.Add(new CasterDataStats("004", 1, true));
            SaveManager.Save(playerData);
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

    public void AddCaster(string _id)
    {

        if (playerData.casterDatabase.Any(item => item.ID == _id))
        {
            //! maybe a duplicate function?
            print("Already Exist");
        }
        else
        {

            for (int i = 0; i < playerInventory.AllCasters.Count; i++)
            {
                if (playerInventory.AllCasters[i].ID == _id)
                {
                    print("exits");
                    playerData.casterDatabase.Add(new CasterDataStats(_id, 1, true));
                    break;
                }
                else
                {
                    print("noexits");
                }
            }
            SaveManager.Save(playerData);
        }
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
